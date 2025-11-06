using System.Text.Json;
using MyApp.Common;
using System.Reflection;
using System.IO;

namespace MyApi.Singletons;

public class MyAppConfig
{
    static AppConfig? appConfig = null;
    private MyAppConfig() { }

    public static AppConfig Instance
    {
        get
        {
            if (appConfig != null)
            {
                return appConfig;  
            }

            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string configDir = Path.Combine(baseDir, "config");
            string fName = Path.Combine(configDir, "config.json");

            if (!File.Exists(fName))
            {
                throw new FileNotFoundException($"Config file not found: {fName}");
            }

            string val = File.ReadAllText(fName);
            Console.WriteLine($"Raw JSON: {val}");

            JsonSerializerOptions opt = new(JsonSerializerOptions.Default)
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                appConfig = JsonSerializer.Deserialize<AppConfig>(val, opt);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to deserialize config: {ex.Message}", ex);
            }

            if (appConfig == null)
            {
                throw new FormatException("Config deserialization returned null.");
            }

            Console.WriteLine($"Deserialized appConfig: {JsonSerializer.Serialize(appConfig, opt)}");

            string dbType = appConfig.DBType ?? "unknown";
            Console.WriteLine($"DBType from config: '{dbType}'");
            Console.WriteLine($"Drivers count: {appConfig.Drivers?.Count ?? 0}");

            if (appConfig.Drivers != null)
            {
                foreach (var d in appConfig.Drivers)
                {
                    Console.WriteLine($"Driver Name: '{d.Name}'");
                }
            }

        
            var driver = appConfig.Drivers?.FirstOrDefault(d => d.Name.Equals(dbType, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"Found driver: {driver?.Name ?? "null"}");

            if (driver == null)
            {
                throw new Exception($"Driver for DBType '{dbType}' not found in config.");
            }

       
            appConfig.DatabaseDriver = driver.Name;
            appConfig.ConnectionString = $"Server={driver.DBConfig?.Location ?? "localhost"};Database={driver.DBConfig?.Database ?? "myapp"};User={driver.DBConfig?.UserName ?? "db"};Password={driver.DBConfig?.Password ?? "pwd"};";

          
            if (!string.IsNullOrEmpty(driver.LIB))
            {
                string resolvedPath = Path.GetFullPath(Path.Combine(baseDir, driver.LIB));
                Console.WriteLine($"Initial resolved DLL path: {resolvedPath}");

                
                if (!File.Exists(resolvedPath))
                {
                    string projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));  
                    string driversPath = Path.Combine(projectRoot, "Drivers", driver.Name, "bin", "Debug", "net9.0", $"{driver.Name}.dll");
                    Console.WriteLine($"Checking fallback path: {driversPath}");
                    if (File.Exists(driversPath))
                    {
                        resolvedPath = driversPath;
                        Console.WriteLine($"Using fallback DLL path: {resolvedPath}");
                    }
                    else
                    {
                        throw new FileNotFoundException($"DLL not found at {resolvedPath} or fallback {driversPath}");
                    }
                }

                try
                {
                    Assembly.LoadFrom(resolvedPath);
                    Console.WriteLine("DLL loaded successfully.");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to load DLL from {resolvedPath}: {ex.Message}", ex);
                }
            }
            else
            {
                Console.WriteLine("Warning: No LIB path specified for driver.");
            }

            Console.WriteLine($"Configuration loaded: DBType = {dbType}, ConnectionString = {appConfig.ConnectionString}");

            return appConfig;
        }
    }
}
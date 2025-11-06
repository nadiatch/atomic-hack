
using System.Reflection;
using MyApp.Common;

namespace MyApi.Singletons
{
    public class DAO
    {
        private static DAO? instance;
        private static readonly object locker = new();

        private IDatabase repo;

        private DAO()
        {
            var config = MyAppConfig.Instance; 

            var driver = config.Drivers?.FirstOrDefault(d =>
                d.Name.Equals(config.DBType, StringComparison.OrdinalIgnoreCase));

            if (driver == null)
                throw new Exception($"Driver for DBType '{config.DBType}' not found.");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == driver.Name);

            if (assembly == null)
            {
                string dllPath = Path.GetFullPath(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    driver.LIB));

                if (!File.Exists(dllPath))
                    throw new FileNotFoundException($"DLL not found: {dllPath}");

                assembly = Assembly.LoadFrom(dllPath);
            }

            var dbType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == "Database" && typeof(IDatabase).IsAssignableFrom(t));

            if (dbType == null)
                throw new Exception("Database type implementing IDatabase not found in assembly.");

            repo = (IDatabase)Activator.CreateInstance(dbType)!;
        }

        public static DAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = new DAO();
                    }
                }
                return instance;
            }
        }

        public IDatabase GetRepository()
        {
            return repo;
        }
    }
}

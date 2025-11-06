namespace MyApp.Common;

public class AppConfig
{
    public string DBType { get; }
    public List<Driver> Drivers { get; set; }
    public string DatabaseDriver { get; set; }
    public string ConnectionString { get; set; }


    public AppConfig(string dbType)
    {
        DBType = dbType;
        Drivers = new List<Driver>();
        DatabaseDriver = string.Empty;
        ConnectionString = string.Empty;
    }
}
namespace MyApp.Common;

public class Driver
{
    public string Name { get; set; }
    public string LIB { get; set; }
    public DBСonfig? DBConfig { get; set; }



    public Driver(string name, string lib)
    {
        Name = name;
        LIB = lib;
    }

    public Driver()
    {
        Name = string.Empty;
        LIB = string.Empty;
    }
}
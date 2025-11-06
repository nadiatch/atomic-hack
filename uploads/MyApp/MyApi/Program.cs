using System.Text.Json;
using MyApi.Singletons;
using MyApp.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
/*var dBDriver = new MyApp.Common.DBDriver("ddd", "fff");
string fileName = "C:/MyApp/MyApi/config/fig.json";
string jsonString = JsonSerializer.Serialize(dBDriver);
File.WriteAllText(fileName, jsonString);
Console.WriteLine(File.ReadAllText(fileName));

string fileName2 = "C:/MyApp/MyApi/config/config.json";
string jsonString2 = File.ReadAllText(fileName2);
MyApp.Common.AppConfig config = JsonSerializer.Deserialize<MyApp.Common.AppConfig>(jsonString2)!;
Console.WriteLine($"DBType: {config.DBType}");*/
var config = MyAppConfig.Instance;
Console.WriteLine($"Конфигурация загружена: DBType = {config.DBType}, ConnectionString = {config.ConnectionString}");
try
{
    var dao = DAO.Instance;

}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при проверке DAO: {ex.Message}");
}

// var DBConfig = new DBСonfig
//             {
//                 Location = "Chelyabinsk",
//                 Database = "MariaDB",
//                 UserName = "ZHUK",
//                 Password = "pwd"
//             };
//             string fileName = "DBConfig.json"; 
//             string jsonString = JsonSerializer.Serialize(DBConfig);
//             File.WriteAllText(fileName, jsonString);
//             Console.WriteLine(File.ReadAllText(fileName));


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Konfigurationsabschnitt für DatabaseSettings laden und registrieren
var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

var app = builder.Build();

app.MapGet("/", () => "Minimal API Version 1.0");

app.MapGet("/check", (IOptions<DatabaseSettings> options) =>
{
    try
    {
        var mongoDbConnectionString = options.Value.ConnectionString;
        Console.WriteLine("Fehler: " + mongoDbConnectionString);
        var client = new MongoClient(mongoDbConnectionString);
        var databases = client.ListDatabaseNames().ToList();
        var databaseNames = string.Join(", ", databases);

        return "Zugriff auf MongoDB ok. Verfügbare Datenbanken" + databaseNames;
    }
    catch (Exception e)
    {
        return ("Fehler beim Zugriff auf MongoDB" + e.Message);
    }
});

app.Run();

/* app.MapGet("/check", () => {
    try
    {
        const string connectionUri = "mongodb://gbs:geheim@localhost:27017";
        // Creates a new client and connects to the server
        var cleint = new MongoClient(connectionUri);
        return "connection ok";
    }
    catch(System.Exception e){
        return "Zugriff auf MongoDB funktioniert nicht: ";
    }
}); */
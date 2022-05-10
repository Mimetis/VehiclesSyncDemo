// See https://aka.ms/new-console-template for more information
using Client;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json", false, true)
  .AddJsonFile("appsettings.local.json", true, true)
  .Build();

var syncUri = configuration.GetSection("Api")["SyncAddress"];
var sqliteConnection = configuration.GetConnectionString("SqliteConnection");

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.Configure<ApiOptions>(options => configuration.Bind("Api", options));

var servicesProvider = services.BuildServiceProvider();

var app = new CommandLineApplication<Board>();

app.Conventions.UseDefaultConventions()
               .UseConstructorInjection(servicesProvider); ;


app.ShowHelp();
do
{
    // Console.Clear();
    Console.WriteLine("Enter you command line arguments:");
    Console.WriteLine();
    try
    {
        var debugArgs = Console.ReadLine();
        if (debugArgs != null)
        {
            var debugArgsArray = debugArgs.Split(" ");
            app.Execute(debugArgsArray);
        }

    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine();
    Console.WriteLine("Hit 'Esc' to end or another key to restart");

} while (true);

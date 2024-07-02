// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Routify.Data;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();  
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddPostgres(hostContext.Configuration);
    });
    
var host = hostBuilder.Build();
host.Run();
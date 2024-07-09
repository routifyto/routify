using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Handlers;
using Routify.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

//inject services
builder.Services.AddSingleton<Repository>();
builder.Services.AddSingleton<LogService>();
builder.Services.AddHostedService<Synchronizer>();
builder.Services.AddHostedService(sp =>
{
    var logService = sp.GetRequiredService<LogService>();
    return logService ?? throw new Exception("LogService is not registered");
});

//inject handlers
builder.Services.AddKeyedScoped<IRequestHandler, TextHandler>(RouteType.Text);

//inject http clients
builder.Services.AddHttpClient("api",client =>
{
    var apiConfig = builder.Configuration.GetSection("Api");
    var baseUrl = apiConfig["BaseUrl"];
    var token = apiConfig["Token"];
    
    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new Exception("BaseUrl is not set.");
    
    if (string.IsNullOrWhiteSpace(token))
        throw new Exception("Token is not set.");
    
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("x-proxy-token", token);
});

builder.Services.AddHttpClient("openai", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");
});

var app = builder.Build();

app.MapPost("/{appId}/{*path}", async (
    IServiceProvider serviceProvider,
    Repository repository,
    string appId,
    string path,
    HttpContext httpContext) =>
{
    var appData = repository.GetApp(appId);
    var routeData = appData?.GetRoute(path);
    if (appData == null || routeData == null)
    {
        httpContext.Response.StatusCode = 404;
        return;
    }
    
    var context = new RequestContext
    {
        HttpContext = httpContext,
        App = appData,
        Route = routeData
    };

    var handler = serviceProvider.GetRequiredKeyedService<IRequestHandler>(routeData.Type);
    await handler.HandleAsync(context);
});

app.Run();
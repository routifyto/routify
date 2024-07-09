using Microsoft.AspNetCore.Mvc;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Handlers;
using Routify.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

//inject services
builder.Services.AddSingleton<Repository>();
builder.Services.AddHostedService<Synchronizer>();

//inject handlers
builder.Services.AddScoped<OpenAiCompletionHandler>();

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
    [FromServices] Repository repository,
    [FromServices] OpenAiCompletionHandler openAiCompletionHandler,
    [FromRoute] string appId,
    [FromRoute] string path,
    HttpContext httpContext) =>
{
    var appData = repository.GetApp(appId);
    if (appData == null)
    {
        httpContext.Response.StatusCode = 404;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(new { message = "App not found" });
        return;
    }
    
    var routeData = appData.GetRoute(path);
    if (routeData == null)
    {
        httpContext.Response.StatusCode = 404;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(new { message = "Route not found" });
        return;
    }

    var context = new RoutifyRequestContext
    {
        HttpContext = httpContext,
        App = appData,
        Route = routeData
    };

    if (routeData.Type == RouteType.Completion)
    {
        await openAiCompletionHandler.Handle(context);
    }
});

app.Run();
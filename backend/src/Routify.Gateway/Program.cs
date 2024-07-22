using Routify.Data.Enums;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Handlers;
using Routify.Gateway.Providers.Anthropic;
using Routify.Gateway.Providers.AzureOpenAi;
using Routify.Gateway.Providers.Cohere;
using Routify.Gateway.Providers.Groq;
using Routify.Gateway.Providers.Mistral;
using Routify.Gateway.Providers.OpenAi;
using Routify.Gateway.Providers.Perplexity;
using Routify.Gateway.Providers.TogetherAi;
using Routify.Gateway.Services;
using Routify.Gateway.Utils;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

//inject services
builder.Services.AddSingleton<Repository>();
builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<CostService>();
builder.Services.AddHostedService<Synchronizer>();
builder.Services.AddHostedService(sp =>
{
    var logService = sp.GetRequiredService<LogService>();
    return logService ?? throw new Exception("LogService is not registered");
});

//inject handlers
builder.Services.AddKeyedScoped<IRequestHandler, CompletionHandler>(RouteType.Completion);

//inject providers
builder.Services.AddOpenAi();
builder.Services.AddTogetherAi();
builder.Services.AddAnthropic();
builder.Services.AddMistral();
builder.Services.AddCohere();
builder.Services.AddGroq();
builder.Services.AddPerplexity();
builder.Services.AddAzureOpenAi();

//inject api http client
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
    client.DefaultRequestHeaders.Add("x-gateway-token", token);
});

//inject redis
builder.Services.AddSingleton<IDatabase>(_ =>
{
    var redisConfig = builder.Configuration.GetSection("Redis");
    var connectionString = redisConfig["ConnectionString"];
    
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new Exception("Redis connection string is not set.");
    
    var redis = ConnectionMultiplexer.Connect(connectionString);
    return redis.GetDatabase();
});

var app = builder.Build();

app.MapPost("/{appId}/{*path}", async (
    IServiceProvider serviceProvider,
    Repository repository,
    string appId,
    string path,
    HttpContext httpContext,
    CacheService cacheService,
    CancellationToken cancellationToken) =>
{
    var appData = repository.GetApp(appId);
    var routeData = appData?.GetRoute(path);
    if (appData == null || routeData == null)
    {
        httpContext.Response.StatusCode = 404;
        return;
    }
    
    var apiKey = AuthorizationUtils.ParseApiKey(httpContext, appData);
    if (apiKey == null)
    {
        httpContext.Response.StatusCode = 401;
        return;
    }
    
    var context = new RequestContext
    {
        HttpContext = httpContext,
        App = appData,
        Route = routeData,
        ApiKey = apiKey,
        Cache = cacheService
    };

    var consumerHeader = httpContext.Request.GetConsumer();
    if (!string.IsNullOrWhiteSpace(consumerHeader))
        context.Consumer = appData.GetConsumer(consumerHeader);

    var handler = serviceProvider.GetRequiredKeyedService<IRequestHandler>(routeData.Type);
    await handler.HandleAsync(context, cancellationToken);
});

app.Run();
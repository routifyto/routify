using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Services;
using Routify.Provider.Core.Completion;

namespace Routify.Gateway.Handlers;

internal class CompletionHandler(
    IServiceProvider serviceProvider,
    LogService logService)
    : IRequestHandler
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
    
    public async Task HandleAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        var log = new CompletionLog
        {
            Id = RoutifyId.Generate(IdType.CompletionLog),
            AppId = context.App.Id,
            RouteId = context.Route.Id,
            Path = context.Route.Path,
            StartedAt = DateTime.UtcNow,
            ApiKeyId = ""
        };
        
        try
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            
            var completionSerializer = serviceProvider.GetKeyedService<ICompletionSerializer>(ProviderIds.OpenAi);
            if (completionSerializer == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }
            
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);
            var input = completionSerializer.Parse(requestBody);
            if (input == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            
            var routeProvider = context.Route.Providers.FirstOrDefault();
            if (routeProvider == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }
            
            var appProvider = context.App.GetProviderById(routeProvider.AppProviderId);
            if (appProvider == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }
            
            var completionProvider = serviceProvider.GetKeyedService<ICompletionProvider>(appProvider.Provider);
            if (completionProvider == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }

            var completionRequest = new CompletionRequest
            {
                Timeout = 10,
                Input = input,
                AppProviderAttrs = appProvider.Attrs,
                RouteProviderAttrs = routeProvider.Attrs,
                Model = routeProvider.Model
            };
            
            var completionResponse = await completionProvider.CompleteAsync(completionRequest, cancellationToken);
        
            log.RequestBody = requestBody;
            log.AppProviderId = appProvider.Id;
            log.RouteProviderId = routeProvider.Id;
            log.Provider = appProvider.Provider;
            log.Model = completionResponse.Payload?.Model;
            log.ResponseStatusCode = completionResponse.StatusCode;
            log.InputTokens = completionResponse.InputTokens;
            log.OutputTokens = completionResponse.OutputTokens;
            log.InputCost = completionResponse.InputCost;
            log.OutputCost = completionResponse.OutputCost;
            
            context.HttpContext.Response.StatusCode = completionResponse.StatusCode;
            if (completionResponse.Payload != null)
            {
                var responseBdy = completionSerializer.Serialize(completionResponse.Payload, Options);
                log.ResponseBody = responseBdy;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(responseBdy, cancellationToken);    
            }
        }
        finally
        {
            log.EndedAt = DateTime.UtcNow;
            log.Duration = (log.EndedAt - log.StartedAt).TotalMilliseconds;
            logService.Save(log);
        }
    }
}
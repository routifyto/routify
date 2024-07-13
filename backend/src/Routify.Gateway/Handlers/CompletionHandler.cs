using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Services;

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
            ApiKeyId = context.ApiKey.Id
        };
        
        try
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            
            var serializer = serviceProvider.GetKeyedService<ICompletionSerializer>(ProviderIds.OpenAi);
            if (serializer == null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return;
            }
            
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);
            var input = serializer.Parse(requestBody);
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
            log.Model = completionResponse.Model;
            log.ResponseStatusCode = completionResponse.StatusCode;
            log.InputTokens = completionResponse.InputTokens;
            log.OutputTokens = completionResponse.OutputTokens;
            log.InputCost = completionResponse.InputCost;
            log.OutputCost = completionResponse.OutputCost;
            
            context.HttpContext.Response.StatusCode = completionResponse.StatusCode;
            if (completionResponse.Output != null)
            {
                var outputMapper = serviceProvider.GetKeyedService<ICompletionOutputMapper>(ProviderIds.OpenAi);
                if (outputMapper == null)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    return;
                }
                
                var responseOutput = outputMapper.Map(completionResponse.Output);
                var responseBody = serializer.Serialize(responseOutput, Options);
                log.ResponseBody = responseBody;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(responseBody, cancellationToken);    
            }
            else if (completionResponse.Error != null)
            {
                log.ResponseBody = completionResponse.Error;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(completionResponse.Error, cancellationToken);
            }
            else
            {
                log.ResponseBody = string.Empty;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(string.Empty, cancellationToken);
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
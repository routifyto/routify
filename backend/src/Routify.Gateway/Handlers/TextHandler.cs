using System.Net;
using System.Text.Json;
using Routify.Core.Utils;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.OpenAi;
using Routify.Gateway.Services;

namespace Routify.Gateway.Handlers;

internal class TextHandler(
    IHttpClientFactory httpClientFactory,
    LogService logService)
    : IRequestHandler
{
    public async Task HandleAsync(
        RequestContext context)
    {
        var log = new TextLog
        {
            Id = RoutifyId.Generate(IdType.TextLog),
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
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            log.RequestBody = requestBody;
            
            var input = JsonSerializer.Deserialize<ChatCompletionInput>(requestBody);
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
            
            if (!appProvider.Attrs.TryGetValue("apiKey", out var openAiApiKey))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        
            var client = httpClientFactory.CreateClient(appProvider.Provider);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
        
            var response = await client.PostAsJsonAsync("chat/completions", input);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responsePayload = JsonSerializer.Deserialize<ChatCompletionPayload>(responseBody);
            
            log.AppProviderId = appProvider.Id;
            log.RouteProviderId = routeProvider.Id;
            log.Provider = appProvider.Provider;
            log.Model = input.Model;
            log.ResponseStatusCode = (int)response.StatusCode;
            log.ResponseBody = responseBody;

            log.InputTokens = responsePayload?.Usage.PromptTokens ?? 0;
            log.OutputTokens = responsePayload?.Usage.CompletionTokens ?? 0;
            
            context.HttpContext.Response.StatusCode = (int)response.StatusCode;
            context.HttpContext.Response.ContentType = response.Content.Headers.ContentType?.ToString();
            await context.HttpContext.Response.WriteAsync(responseBody);
        }
        finally
        {
            log.EndedAt = DateTime.UtcNow;
            logService.Save(log);
        }
    }
}
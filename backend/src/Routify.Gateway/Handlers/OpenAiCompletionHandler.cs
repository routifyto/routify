using System.Net;
using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.OpenAi;

namespace Routify.Gateway.Handlers;

internal class OpenAiCompletionHandler(
    IHttpClientFactory httpClientFactory)
{
    public async Task Handle(
        RoutifyRequestContext context)
    {
        var httpContext = context.HttpContext;
        var request = httpContext.Request;
        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
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
        
        var client = httpClientFactory.CreateClient("openai");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
        
        var response = await client.PostAsJsonAsync("chat/completions", input);
        context.HttpContext.Response.StatusCode = (int)response.StatusCode;
        context.HttpContext.Response.ContentType = response.Content.Headers.ContentType?.ToString();
        var responseBody = await response.Content.ReadAsStringAsync();
        await context.HttpContext.Response.WriteAsync(responseBody);
    }
}
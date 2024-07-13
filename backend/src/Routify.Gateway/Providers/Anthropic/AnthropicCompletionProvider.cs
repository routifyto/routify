using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.Anthropic)] ICompletionInputMapper inputMapper)
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        { "claude-3-5-sonnet-20240620", 3m },
        { "claude-3-opus-20240229", 15m },
        { "claude-3-sonnet-20240229", 3m },
        { "claude-3-haiku-20240307", 0.25m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "claude-3-5-sonnet-20240620", 15m },
        { "claude-3-opus-20240229", 75m },
        { "claude-3-sonnet-20240229", 5m },
        { "claude-3-haiku-20240307", 1.25m },
    };

    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request,
        CancellationToken cancellationToken)
    {
        if (!request.AppProviderAttrs.TryGetValue("apiKey", out var apiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        var client = httpClientFactory.CreateClient(ProviderIds.Anthropic);
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var anthropicInput = inputMapper.Map(request.Input) as AnthropicCompletionInput;
        if (anthropicInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }

        if (!string.IsNullOrWhiteSpace(request.Model))
            anthropicInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            anthropicInput.System = systemPrompt;
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            anthropicInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            anthropicInput.MaxTokens = maxTokens;
        }

        var response = await client.PostAsJsonAsync("messages", anthropicInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new CompletionResponse
            {
                StatusCode = (int)response.StatusCode,
                Error = responseBody,
            };
        }

        var responseOutput = JsonSerializer.Deserialize<AnthropicCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        var usage = responseOutput.Usage;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = responseOutput.Model,
            InputTokens = usage.InputTokens,
            OutputTokens = usage.OutputTokens,
            Output = responseOutput,
            InputCost = CalculateInputCost(responseOutput.Model, usage.InputTokens),
            OutputCost = CalculateOutputCost(responseOutput.Model, usage.OutputTokens)
        };

        return completionResponse;
    }

    private static decimal CalculateInputCost(
        string model,
        int tokens)
    {
        if (ModelInputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }

    private static decimal CalculateOutputCost(
        string model,
        int tokens)
    {
        if (ModelOutputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }
}
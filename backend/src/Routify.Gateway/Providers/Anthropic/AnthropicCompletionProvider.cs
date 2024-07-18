using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Anthropic.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<AnthropicCompletionInput, AnthropicCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "claude-3-5-sonnet-20240620", new CompletionModel
            {
                Id = "claude-3-5-sonnet-20240620",
                InputCost = 3m,
                OutputCost = 15m,
            }
        },
        {
            "claude-3-opus-20240229", new CompletionModel
            {
                Id = "claude-3-opus-20240229",
                InputCost = 15m,
                OutputCost = 75m,
            }
        },
        {
            "claude-3-sonnet-20240229", new CompletionModel
            {
                Id = "claude-3-sonnet-20240229",
                InputCost = 3m,
                OutputCost = 5m,
            }
        },
        {
            "claude-3-haiku-20240307", new CompletionModel
            {
                Id = "claude-3-haiku-20240307",
                InputCost = 0.25m,
                OutputCost = 1.25m,
            }
        }
    };
    
    public override string Id => ProviderIds.Anthropic;
    
    public override Dictionary<string, CompletionModel> Models => _models;
    
    protected override HttpClient PrepareHttpClient(
        CompletionRequest request)
    {
        var apiKey = request.AppProvider.Attrs.GetValueOrDefault("apiKey");
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new GatewayException(HttpStatusCode.Unauthorized);
        
        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        
        return client;
    }

    protected override string PrepareRequestUrl(
        CompletionRequest request)
    {
        return "messages";
    }

    protected override string GetModel(
        AnthropicCompletionInput input, 
        AnthropicCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        AnthropicCompletionOutput output)
    {
        return output.Usage.InputTokens;
    }
    
    protected override int GetOutputTokens(
        AnthropicCompletionOutput output)
    {
        return output.Usage.OutputTokens;
    }

    protected override AnthropicCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var anthropicInput = AnthropicCompletionInputMapper.Map(request.Input);
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            anthropicInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            anthropicInput.System = systemPrompt;
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            anthropicInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            anthropicInput.MaxTokens = maxTokens;
        }
        
        return anthropicInput;
    }
    
    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<AnthropicCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = AnthropicCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}

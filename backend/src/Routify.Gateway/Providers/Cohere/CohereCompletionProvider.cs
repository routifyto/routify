using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Cohere.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionProvider(
    IHttpClientFactory httpClientFactory) 
    : CompletionProviderBase<CohereCompletionInput, CohereCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "command-r-plus", new CompletionModel
            {
                Id = "command-r-plus",
                InputCost = 300m,
                OutputCost = 1500m
            }
        },
        {
            "command-r", new CompletionModel
            {
                Id = "command-r",
                InputCost = 500m,
                OutputCost = 150m
            }
        },
        {
            "command", new CompletionModel
            {
                Id = "command",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "command-nightly", new CompletionModel
            {
                Id = "command-nightly",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "command-light", new CompletionModel
            {
                Id = "command-light",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "command-light-nightly", new CompletionModel
            {
                Id = "command-light-nightly",
                InputCost = 0m,
                OutputCost = 0m
            }
        }
    };
    
    public override string Id => ProviderIds.Cohere;
    public override Dictionary<string, CompletionModel> Models => _models;

    protected override HttpClient PrepareHttpClient(
        CompletionRequest request)
    {
        if (!request.AppProvider.Attrs.TryGetValue("apiKey", out var apiKey))
            throw new GatewayException(HttpStatusCode.Unauthorized);
        
        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        return client;
    }

    protected override string PrepareRequestUrl(
        CompletionRequest request)
    {
        return "chat";
    }

    protected override string GetModel(
        CohereCompletionInput input, 
        CohereCompletionOutput output)
    {
        return input.Model ?? string.Empty;
    }
    
    protected override int GetInputTokens(
        CohereCompletionOutput output)
    {
        return output.Meta?.BilledUnits?.InputTokens ?? 0;
    }
    
    protected override int GetOutputTokens(
        CohereCompletionOutput output)
    {
        return output.Meta?.BilledUnits?.OutputTokens ?? 0;
    }

    protected override CohereCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var cohereInput = CohereCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            cohereInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            cohereInput.ChatHistory.Insert(0, new CohereCompletionMessageInput
            {
                Message = systemPrompt,
                Role = "SYSTEM"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            cohereInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            cohereInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            cohereInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            cohereInput.PresencePenalty = presencePenalty;
        }

        return cohereInput;
    }
    
    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<CohereCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = CohereCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
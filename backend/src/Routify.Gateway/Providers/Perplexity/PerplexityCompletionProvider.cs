using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Perplexity.Models;

namespace Routify.Gateway.Providers.Perplexity;

internal class PerplexityCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<PerplexityCompletionInput, PerplexityCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "llama-3-sonar-small-32k-online", new CompletionModel
            {
                Id = "llama-3-sonar-small-32k-online",
                InputCost = 20m,
                OutputCost = 20m
            }
        },
        {
            "llama-3-sonar-small-32k-chat", new CompletionModel
            {
                Id = "llama-3-sonar-small-32k-chat",
                InputCost = 20m,
                OutputCost = 20m
            }
        },
        {
            "llama-3-sonar-large-32k-online", new CompletionModel
            {
                Id = "llama-3-sonar-large-32k-online",
                InputCost = 100m,
                OutputCost = 100m
            }
        },
        {
            "llama-3-sonar-large-32k-chat", new CompletionModel
            {
                Id = "llama-3-sonar-large-32k-chat",
                InputCost = 100m,
                OutputCost = 100m
            }
        },
        {
            "llama-3-8b-instruct", new CompletionModel
            {
                Id = "llama-3-8b-instruct",
                InputCost = 20m,
                OutputCost = 20m
            }
        },
        {
            "llama-3-70b-instruct", new CompletionModel
            {
                Id = "llama-3-70b-instruct",
                InputCost = 100m,
                OutputCost = 100m
            }
        },
        {
            "mixtral-8x7b-instruct", new CompletionModel
            {
                Id = "mixtral-8x7b-instruct",
                InputCost = 60m,
                OutputCost = 60m
            }
        },
    };

    public override string Id => ProviderIds.Perplexity;
    
    public override Dictionary<string, CompletionModel> Models => _models;
    
    protected override HttpClient PrepareHttpClient(
        CompletionRequest request)
    {
        var apiKey = request.AppProvider.Attrs.GetValueOrDefault("apiKey");
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new GatewayException(HttpStatusCode.Unauthorized);
        
        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        return client;
    }

    protected override string PrepareRequestUrl(
        CompletionRequest request)
    {
        return "chat/completions";
    }

    protected override string GetModel(
        PerplexityCompletionInput input, 
        PerplexityCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        PerplexityCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }
    
    protected override int GetOutputTokens(
        PerplexityCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }

    protected override PerplexityCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var perplexityInput = PerplexityCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            perplexityInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            perplexityInput.Messages.Insert(0, new PerplexityCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            perplexityInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            perplexityInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString)
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString)
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            perplexityInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString)
            && !string.IsNullOrWhiteSpace(presencePenaltyString)
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            perplexityInput.PresencePenalty = presencePenalty;
        }

        return perplexityInput;
    }

    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<PerplexityCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = PerplexityCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
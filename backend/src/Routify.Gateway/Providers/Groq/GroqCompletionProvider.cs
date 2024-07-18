using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Groq.Models;

namespace Routify.Gateway.Providers.Groq;

internal class GroqCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<GroqCompletionInput, GroqCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "llama3-8b-8192", new CompletionModel
            {
                Id = "llama3-8b-8192",
                InputCost = 0.05m,
                OutputCost = 0.08m
            }
        },
        {
            "llama3-70b-8192", new CompletionModel
            {
                Id = "llama3-70b-8192",
                InputCost = 0.59m,
                OutputCost = 0.79m
            }
        },

        {
            "mixtral-8x7b-32768", new CompletionModel
            {
                Id = "mixtral-8x7b-32768",
                InputCost = 0.24m,
                OutputCost = 0.24m
            }
        },
        {
            "gemma-7b-it", new CompletionModel
            {
                Id = "gemma-7b-it",
                InputCost = 0.07m,
                OutputCost = 0.07m
            }
        },
        {
            "gemma2-9b-it", new CompletionModel
            {
                Id = "gemma2-9b-it",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
    };

    public override string Id => ProviderIds.Groq;
    
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
        return "chat/completions";
    }

    protected override string GetModel(
        GroqCompletionInput input, 
        GroqCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        GroqCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }
    
    protected override int GetOutputTokens(
        GroqCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }

    protected override GroqCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var groqInput = GroqCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            groqInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            groqInput.Messages.Insert(0, new GroqCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            groqInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            groqInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString)
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString)
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            groqInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString)
            && !string.IsNullOrWhiteSpace(presencePenaltyString)
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            groqInput.PresencePenalty = presencePenalty;
        }

        return groqInput;
    }

    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<GroqCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var groqOutput = GroqCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(groqOutput);
    }
}
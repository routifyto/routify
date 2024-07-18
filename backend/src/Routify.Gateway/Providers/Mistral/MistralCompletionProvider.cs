using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Mistral.Models;

namespace Routify.Gateway.Providers.Mistral;

internal class MistralCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<MistralCompletionInput, MistralCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "open-mistral-7b", new CompletionModel
            {
                Id = "open-mistral-7b",
                InputCost = 0.25m,
                OutputCost = 1m
            }
        },
        {
            "open-mixtral-8x7b", new CompletionModel
            {
                Id = "open-mixtral-8x7b",
                InputCost = 0.7m,
                OutputCost = 0.7m
            }
        },
        {
            "open-mixtral-8x22b", new CompletionModel
            {
                Id = "open-mixtral-8x22b",
                InputCost = 2m,
                OutputCost = 6m
            }
        },
        {
            "mistral-small-2402", new CompletionModel
            {
                Id = "mistral-small-2402",
                InputCost = 1m,
                OutputCost = 3m
            }
        },
        {
            "codestral-2405", new CompletionModel
            {
                Id = "codestral-2405",
                InputCost = 1m,
                OutputCost = 3m
            }
        },
        {
            "mistral-large-2402", new CompletionModel
            {
                Id = "mistral-large-2402",
                InputCost = 4m,
                OutputCost = 12m
            }
        }
    };

    public override string Id => ProviderIds.Mistral;
    
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
        MistralCompletionInput input, 
        MistralCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        MistralCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }
    
    protected override int GetOutputTokens(
        MistralCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }
    protected override MistralCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var mistralAiInput = MistralCompletionInputMapper.Map(request.Input);
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            mistralAiInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            mistralAiInput.Messages.Insert(0, new MistralCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            mistralAiInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            mistralAiInput.MaxTokens = maxTokens;
        }
        
        return mistralAiInput;
    }
    
    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<MistralCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = MistralCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.OpenAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<OpenAiCompletionInput, OpenAiCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "gpt-4o", new CompletionModel
            {
                Id = "gpt-4o",
                InputCost = 500m,
                OutputCost = 1500m
            }
        },
        {
            "gpt-4o-2024-05-13", new CompletionModel
            {
                Id = "gpt-4o-2024-05-13",
                InputCost = 500m,
                OutputCost = 1500m
            }
        },

        {
            "gpt-4-turbo", new CompletionModel
            {
                Id = "gpt-4-turbo",
                InputCost = 1000m,
                OutputCost = 3000m
            }
        },
        {
            "gpt-4-turbo-2024-04-09", new CompletionModel
            {
                Id = "gpt-4-turbo-2024-04-09",
                InputCost = 1000m,
                OutputCost = 3000m
            }
        },
        {
            "gpt-4-turbo-preview", new CompletionModel
            {
                Id = "gpt-4-turbo-preview",
                InputCost = 1000m,
                OutputCost = 3000m
            }
        },
        {
            "gpt-4-0125-preview", new CompletionModel
            {
                Id = "gpt-4-0125-preview",
                InputCost = 1000m,
                OutputCost = 3000m
            }
        },
        {
            "gpt-4", new CompletionModel
            {
                Id = "gpt-4",
                InputCost = 3000m,
                OutputCost = 6000m
            }
        },
        {
            "gpt-4-0613", new CompletionModel
            {
                Id = "gpt-4-0613",
                InputCost = 3000m,
                OutputCost = 6000m
            }
        },
        {
            "gpt-4-0314", new CompletionModel
            {
                Id = "gpt-4-0314",
                InputCost = 3000m,
                OutputCost = 6000m
            }
        },

        {
            "gpt-3.5-turbo-0125", new CompletionModel
            {
                Id = "gpt-3.5-turbo-0125",
                InputCost = 500m,
                OutputCost = 150m
            }
        },
        {
            "gpt-3.5-turbo", new CompletionModel
            {
                Id = "gpt-3.5-turbo",
                InputCost = 50m,
                OutputCost = 150m
            }
        },
        {
            "gpt-3.5-turbo-1106", new CompletionModel
            {
                Id = "gpt-3.5-turbo-1106",
                InputCost = 100m,
                OutputCost = 200m
            }
        },
        {
            "gpt-3.5-turbo-instruct", new CompletionModel
            {
                Id = "gpt-3.5-turbo-instruct",
                InputCost = 150m,
                OutputCost = 200m
            }
        },
    };

    public override string Id => ProviderIds.OpenAi;
    
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
        OpenAiCompletionInput input, 
        OpenAiCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        OpenAiCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }
    
    protected override int GetOutputTokens(
        OpenAiCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }

    protected override OpenAiCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var openAiInput = OpenAiCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            openAiInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            openAiInput.Messages.Insert(0, new OpenAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            openAiInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            openAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString)
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString)
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            openAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString)
            && !string.IsNullOrWhiteSpace(presencePenaltyString)
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            openAiInput.PresencePenalty = presencePenalty;
        }

        return openAiInput;
    }

    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<OpenAiCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = OpenAiCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
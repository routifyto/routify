using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.Perplexity.Models;

namespace Routify.Gateway.Providers.Perplexity;

internal class PerplexityCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
    {
        {
            "llama-3-sonar-small-32k-online", new CompletionModel
            {
                Id = "llama-3-sonar-small-32k-online",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "llama-3-sonar-small-32k-chat", new CompletionModel
            {
                Id = "llama-3-sonar-small-32k-chat",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "llama-3-sonar-large-32k-online", new CompletionModel
            {
                Id = "llama-3-sonar-large-32k-online",
                InputCost = 1m,
                OutputCost = 1m
            }
        },
        {
            "llama-3-sonar-large-32k-chat", new CompletionModel
            {
                Id = "llama-3-sonar-large-32k-chat",
                InputCost = 1m,
                OutputCost = 1m
            }
        },
        {
            "llama-3-8b-instruct", new CompletionModel
            {
                Id = "llama-3-8b-instruct",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "llama-3-70b-instruct", new CompletionModel
            {
                Id = "llama-3-70b-instruct",
                InputCost = 1m,
                OutputCost = 1m
            }
        },
        {
            "mixtral-8x7b-instruct", new CompletionModel
            {
                Id = "mixtral-8x7b-instruct",
                InputCost = 0.6m,
                OutputCost = 0.6m
            }
        },
    };

    public string Id => ProviderIds.OpenAi;

    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request,
        CancellationToken cancellationToken)
    {
        if (!request.AppProvider.Attrs.TryGetValue("apiKey", out var apiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var perplexityInput = PrepareInput(request);
        var requestJson = RoutifyJsonSerializer.Serialize(perplexityInput);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("chat/completions", requestContent, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var requestLog = response.RequestMessage?.ToRequestLog(requestJson);
        var responseLog = response.ToResponseLog(responseBody);

        if (!response.IsSuccessStatusCode)
        {
            return new CompletionResponse
            {
                StatusCode = (int)response.StatusCode,
                Error = responseBody,
                RequestLog = requestLog,
                ResponseLog = responseLog
            };
        }

        var responseOutput = RoutifyJsonSerializer.Deserialize<PerplexityCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                RequestLog = requestLog,
                ResponseLog = responseLog
            };
        }

        var usage = responseOutput.Usage;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = responseOutput.Model,
            InputTokens = usage.PromptTokens,
            OutputTokens = usage.CompletionTokens,
            Output = responseOutput,
            RequestLog = requestLog,
            ResponseLog = responseLog
        };

        if (Models.TryGetValue(responseOutput.Model, out var model))
        {
            completionResponse.InputCost = model.InputCost / model.InputCostUnit * usage.PromptTokens;
            completionResponse.OutputCost = model.OutputCost / model.OutputCostUnit * usage.CompletionTokens;
        }

        return completionResponse;
    }

    private static PerplexityCompletionInput PrepareInput(
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

    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<PerplexityCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = PerplexityCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
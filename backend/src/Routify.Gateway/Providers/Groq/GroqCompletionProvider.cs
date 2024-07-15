using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.OpenAi.Models;

namespace Routify.Gateway.Providers.Groq;

internal class GroqCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
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

    public string Id => ProviderIds.Groq;

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

        var groqInput = PrepareInput(request);
        var requestJson = RoutifyJsonSerializer.Serialize(groqInput);
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

        var responseOutput = RoutifyJsonSerializer.Deserialize<OpenAiCompletionOutput>(responseBody);
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

    private static GroqCompletionInput PrepareInput(
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

    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<GroqCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var groqOutput = GroqCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(groqOutput);
    }
}
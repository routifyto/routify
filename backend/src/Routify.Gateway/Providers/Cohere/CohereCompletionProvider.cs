using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.Cohere.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionProvider(
    IHttpClientFactory httpClientFactory) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
    {
        {
            "command-r-plus", new CompletionModel
            {
                Id = "command-r-plus",
                InputCost = 3m,
                OutputCost = 15m
            }
        },
        {
            "command-r", new CompletionModel
            {
                Id = "command-r",
                InputCost = 0.5m,
                OutputCost = 1.5m
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
    
    public string Id => ProviderIds.Cohere;
    
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

        var client = httpClientFactory.CreateClient(ProviderIds.OpenAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var cohereInput = PrepareInput(request);
        var requestJson = RoutifyJsonSerializer.Serialize(cohereInput);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("chat", requestContent, cancellationToken);
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

        var responseOutput = RoutifyJsonSerializer.Deserialize<CohereCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                RequestLog = requestLog,
                ResponseLog = responseLog
            };
        }

        var usage = responseOutput.Meta?.BilledUnits;
        var model = cohereInput.Model;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = cohereInput.Model,
            Output = responseOutput,
            RequestLog = requestLog,
            ResponseLog = responseLog
        };

        if (string.IsNullOrWhiteSpace(model) || usage == null) 
            return completionResponse;
        
        completionResponse.InputTokens = usage.InputTokens;
        completionResponse.OutputTokens = usage.OutputTokens;
        
        if (Models.TryGetValue(model, out var modelData))
        {
            completionResponse.InputCost = modelData.InputCost / modelData.InputCostUnit * usage.InputTokens;
            completionResponse.OutputCost = modelData.OutputCost / modelData.OutputCostUnit * usage.OutputTokens;
        }

        return completionResponse;
    }

    private static CohereCompletionInput PrepareInput(
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
    
    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<CohereCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = CohereCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
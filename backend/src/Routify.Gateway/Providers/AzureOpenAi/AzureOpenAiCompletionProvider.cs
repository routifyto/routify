using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.AzureOpenAi.Models;

namespace Routify.Gateway.Providers.AzureOpenAi;

internal class AzureOpenAiCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
    {
        {
            "gpt-4o", new CompletionModel
            {
                Id = "gpt-4o",
                InputCost = 5m,
                OutputCost = 15m
            }
        },
        {
            "gpt-4o-2024-05-13", new CompletionModel
            {
                Id = "gpt-4o-2024-05-13",
                InputCost = 5m,
                OutputCost = 15m
            }
        },

        {
            "gpt-4-turbo", new CompletionModel
            {
                Id = "gpt-4-turbo",
                InputCost = 10m,
                OutputCost = 30m
            }
        },
        {
            "gpt-4-turbo-2024-04-09", new CompletionModel
            {
                Id = "gpt-4-turbo-2024-04-09",
                InputCost = 10m,
                OutputCost = 30m
            }
        },
        {
            "gpt-4-turbo-preview", new CompletionModel
            {
                Id = "gpt-4-turbo-preview",
                InputCost = 10m,
                OutputCost = 30m
            }
        },
        {
            "gpt-4-0125-preview", new CompletionModel
            {
                Id = "gpt-4-0125-preview",
                InputCost = 10m,
                OutputCost = 30m
            }
        },
        {
            "gpt-4", new CompletionModel
            {
                Id = "gpt-4",
                InputCost = 30m,
                OutputCost = 60m
            }
        },
        {
            "gpt-4-0613", new CompletionModel
            {
                Id = "gpt-4-0613",
                InputCost = 30m,
                OutputCost = 60m
            }
        },
        {
            "gpt-4-0314", new CompletionModel
            {
                Id = "gpt-4-0314",
                InputCost = 30m,
                OutputCost = 60m
            }
        },

        {
            "gpt-3.5-turbo-0125", new CompletionModel
            {
                Id = "gpt-3.5-turbo-0125",
                InputCost = 0.5m,
                OutputCost = 1.5m
            }
        },
        {
            "gpt-3.5-turbo", new CompletionModel
            {
                Id = "gpt-3.5-turbo",
                InputCost = 0.5m,
                OutputCost = 1.5m
            }
        },
        {
            "gpt-3.5-turbo-1106", new CompletionModel
            {
                Id = "gpt-3.5-turbo-1106",
                InputCost = 1m,
                OutputCost = 2m
            }
        },
        {
            "gpt-3.5-turbo-instruct", new CompletionModel
            {
                Id = "gpt-3.5-turbo-instruct",
                InputCost = 1.5m,
                OutputCost = 2m
            }
        },
    };

    public string Id => ProviderIds.OpenAi;

    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request,
        CancellationToken cancellationToken)
    {
        if (!request.AppProvider.Attrs.TryGetValue("endpoint", out var endpoint))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.ServiceUnavailable,
            };
        }
        
        if (!request.AppProvider.Attrs.TryGetValue("apiKey", out var apiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }
        
        if (!request.AppProvider.Attrs.TryGetValue("apiVersion", out var apiVersion))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.ServiceUnavailable,
            };
        }

        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        var azureOpenAiInput = PrepareInput(request);
        var requestUrl = $"{endpoint}/openai/deployments/{azureOpenAiInput.Model}/chat/completions?api-version={apiVersion}";
        var requestJson = RoutifyJsonSerializer.Serialize(azureOpenAiInput);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(requestUrl, requestContent, cancellationToken);
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

        var responseOutput = RoutifyJsonSerializer.Deserialize<AzureOpenAiCompletionOutput>(responseBody);
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
        var modelUsed = responseOutput.Model ?? azureOpenAiInput.Model ?? string.Empty;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = modelUsed,
            InputTokens = usage.PromptTokens,
            OutputTokens = usage.CompletionTokens,
            Output = responseOutput,
            RequestLog = requestLog,
            ResponseLog = responseLog
        };

        if (Models.TryGetValue(modelUsed, out var model))
        {
            completionResponse.InputCost = model.InputCost / model.InputCostUnit * usage.PromptTokens;
            completionResponse.OutputCost = model.OutputCost / model.OutputCostUnit * usage.CompletionTokens;
        }

        return completionResponse;
    }

    private static AzureOpenAiCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var azureOpenAiInput = AzureOpenAiCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            azureOpenAiInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            azureOpenAiInput.Messages.Insert(0, new AzureOpenAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            azureOpenAiInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            azureOpenAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString)
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString)
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            azureOpenAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString)
            && !string.IsNullOrWhiteSpace(presencePenaltyString)
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            azureOpenAiInput.PresencePenalty = presencePenalty;
        }

        return azureOpenAiInput;
    }

    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<AzureOpenAiCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = AzureOpenAiCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
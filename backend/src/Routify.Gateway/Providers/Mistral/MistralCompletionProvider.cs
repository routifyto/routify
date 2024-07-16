using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.Mistral.Models;

namespace Routify.Gateway.Providers.Mistral;

internal class MistralCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
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

    public string Id => ProviderIds.Mistral;
    
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

        var mistralAiInput = PrepareInput(request);
        var requestJson = RoutifyJsonSerializer.Serialize(mistralAiInput);
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

        var responseOutput = RoutifyJsonSerializer.Deserialize<MistralCompletionOutput>(responseBody);
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

    private static MistralCompletionInput PrepareInput(
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
    
    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<MistralCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = MistralCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
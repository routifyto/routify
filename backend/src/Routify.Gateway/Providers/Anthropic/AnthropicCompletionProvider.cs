using System.Net;
using System.Text;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Providers.Anthropic.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : ICompletionProvider
{
    private static readonly Dictionary<string, CompletionModel> Models = new()
    {
        {
            "claude-3-5-sonnet-20240620", new CompletionModel
            {
                Id = "claude-3-5-sonnet-20240620",
                InputCost = 3m,
                OutputCost = 15m,
            }
        },
        {
            "claude-3-opus-20240229", new CompletionModel
            {
                Id = "claude-3-opus-20240229",
                InputCost = 15m,
                OutputCost = 75m,
            }
        },
        {
            "claude-3-sonnet-20240229", new CompletionModel
            {
                Id = "claude-3-sonnet-20240229",
                InputCost = 3m,
                OutputCost = 5m,
            }
        },
        {
            "claude-3-haiku-20240307", new CompletionModel
            {
                Id = "claude-3-haiku-20240307",
                InputCost = 0.25m,
                OutputCost = 1.25m,
            }
        }
    };

    public string Id => ProviderIds.Anthropic;
    
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
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);


        var anthropicInput = PrepareInput(request);
        var requestJson = RoutifyJsonSerializer.Serialize(anthropicInput);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync("messages", requestContent, cancellationToken);
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

        var responseOutput = RoutifyJsonSerializer.Deserialize<AnthropicCompletionOutput>(responseBody);
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
            InputTokens = usage.InputTokens,
            OutputTokens = usage.OutputTokens,
            Output = responseOutput,
            RequestLog = requestLog,
            ResponseLog = responseLog
        };
        
        if (Models.TryGetValue(responseOutput.Model, out var model))
        {
            completionResponse.InputCost = model.InputCost / model.InputCostUnit * usage.InputTokens;
            completionResponse.OutputCost = model.OutputCost / model.OutputCostUnit * usage.OutputTokens;
        }

        return completionResponse;
    }

    private static AnthropicCompletionInput PrepareInput(
        CompletionRequest request)
    {
        
        var anthropicInput = AnthropicCompletionInputMapper.Map(request.Input);
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            anthropicInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            anthropicInput.System = systemPrompt;
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            anthropicInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            anthropicInput.MaxTokens = maxTokens;
        }
        
        return anthropicInput;
    }
    
    public ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<AnthropicCompletionInput>(input);
    }

    public string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = AnthropicCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}

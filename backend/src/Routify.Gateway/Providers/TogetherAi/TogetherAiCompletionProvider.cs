using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.TogetherAi)] ICompletionInputMapper inputMapper) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        { "Qwen/Qwen2-72B-Instruct", 0.9m },
        { "meta-llama/Llama-3-70b-chat-hf", 0.9m },
        { "Snowflake/snowflake-arctic-instruct", 2.4m },
        { "meta-llama/Llama-3-8b-chat-hf", 0.2m },
        { "microsoft/WizardLM-2-8x22B", 1.2m },
        { "togethercomputer/StripedHyena-Nous-7B", 0.2m },
        { "databricks/dbrx-instruct", 1.2m },
        { "allenai/OLMo-7B-Instruct", 0.2m },
        { "deepseek-ai/deepseek-llm-67b-chat", 0.9m },
        { "google/gemma-7b-it", 0.2m },
        { "google/gemma-2b-it", 0.1m },
        { "NousResearch/Nous-Hermes-2-Mistral-7B-DPO", 0.2m },
        { "NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT", 0.6m },
        { "NousResearch/Nous-Hermes-2-Yi-34B", 0.8m },
        { "codellama/CodeLlama-70b-Instruct-hf", 0.9m },
        { "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO", 0.6m },
        { "snorkelai/Snorkel-Mistral-PairRM-DPO", 0.2m },
        { "deepseek-ai/deepseek-coder-33b-instruct", 0.8m },
        { "zero-one-ai/Yi-34B-Chat", 0.8m },
        { "NousResearch/Nous-Hermes-Llama2-13b", 0.3m },
        { "NousResearch/Nous-Hermes-llama-2-7b", 0.2m },
        { "togethercomputer/Llama-2-7B-32K-Instruct", 0.2m },
        { "meta-llama/Llama-2-70b-chat-hf", 0.9m },
        { "meta-llama/Llama-2-13b-chat-hf", 0.22m },
        { "meta-llama/Llama-2-7b-chat-hf", 0.2m },
        { "codellama/CodeLlama-13b-Instruct-hf", 0.22m },
        { "codellama/CodeLlama-34b-Instruct-hf", 0.78m },
        { "codellama/CodeLlama-7b-Instruct-hf", 0.2m },
        { "NousResearch/Nous-Capybara-7B-V1p9", 0.2m },
        { "teknium/OpenHermes-2p5-Mistral-7B", 0.2m },
        { "Open-Orca/Mistral-7B-OpenOrca", 0.2m },
        { "teknium/OpenHermes-2-Mistral-7B", 0.2m },
        { "Austism/chronos-hermes-13b", 0.3m },
        { "garage-bAInd/Platypus2-70B-instruct", 0.9m },
        { "Gryphe/MythoMax-L2-13b", 0.3m },
        { "togethercomputer/alpaca-7b", 0.2m },
        { "WizardLM/WizardLM-13B-V1.2", 0.2m },
        { "upstage/SOLAR-10.7B-Instruct-v1.0", 0.3m },
        { "OpenAssistant/llama2-70b-oasst-sft-v10", 0.9m },
        { "openchat/openchat-3.5-1210", 0.2m },
        { "Qwen/Qwen1.5-7B-Chat", 0.2m },
        { "Qwen/Qwen1.5-14B-Chat", 0.3m },
        { "Qwen/Qwen1.5-1.8B-Chat", 0.1m },
        { "cognitivecomputations/dolphin-2.5-mixtral-8x7b", 0.6m },
        { "mistralai/Mixtral-8x22B-Instruct-v0.1", 1.2m },
        { "lmsys/vicuna-13b-v1.5", 0.3m },
        { "Qwen/Qwen1.5-0.5B-Chat", 0.1m },
        { "Qwen/Qwen1.5-4B-Chat", 0.1m },
        { "mistralai/Mistral-7B-Instruct-v0.1", 0.2m },
        { "mistralai/Mistral-7B-Instruct-v0.2", 0.2m },
        { "togethercomputer/Pythia-Chat-Base-7B", 0.2m },
        { "Qwen/Qwen1.5-32B-Chat", 0.8m },
        { "Qwen/Qwen1.5-72B-Chat", 0.9m },
        { "mistralai/Mistral-7B-Instruct-v0.3", 0.2m },
        { "Qwen/Qwen1.5-110B-Chat", 1.8m },
        { "mistralai/Mixtral-8x7B-Instruct-v0.1", 0.6m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "Qwen/Qwen2-72B-Instruct", 0.9m },
        { "meta-llama/Llama-3-70b-chat-hf", 0.9m },
        { "Snowflake/snowflake-arctic-instruct", 2.4m },
        { "meta-llama/Llama-3-8b-chat-hf", 0.2m },
        { "microsoft/WizardLM-2-8x22B", 1.2m },
        { "databricks/dbrx-instruct", 1.2m },
        { "allenai/OLMo-7B-Instruct", 0.2m },
        { "deepseek-ai/deepseek-llm-67b-chat", 0.9m },
        { "google/gemma-7b-it", 0.2m },
        { "google/gemma-2b-it", 0.1m },
        { "NousResearch/Nous-Hermes-2-Mistral-7B-DPO", 0.2m },
        { "NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT", 0.6m },
        { "NousResearch/Nous-Hermes-2-Yi-34B", 0.8m },
        { "codellama/CodeLlama-70b-Instruct-hf", 0.9m },
        { "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO", 0.6m },
        { "snorkelai/Snorkel-Mistral-PairRM-DPO", 0.2m },
        { "deepseek-ai/deepseek-coder-33b-instruct", 0.8m },
        { "zero-one-ai/Yi-34B-Chat", 0.8m },
        { "NousResearch/Nous-Hermes-Llama2-13b", 0.3m },
        { "NousResearch/Nous-Hermes-llama-2-7b", 0.2m },
        { "togethercomputer/Llama-2-7B-32K-Instruct", 0.2m },
        { "meta-llama/Llama-2-70b-chat-hf", 0.9m },
        { "meta-llama/Llama-2-13b-chat-hf", 0.22m },
        { "meta-llama/Llama-2-7b-chat-hf", 0.2m },
        { "codellama/CodeLlama-13b-Instruct-hf", 0.22m },
        { "codellama/CodeLlama-34b-Instruct-hf", 0.78m },
        { "codellama/CodeLlama-7b-Instruct-hf", 0.2m },
        { "NousResearch/Nous-Capybara-7B-V1p9", 0.2m },
        { "teknium/OpenHermes-2p5-Mistral-7B", 0.2m },
        { "Open-Orca/Mistral-7B-OpenOrca", 0.2m },
        { "teknium/OpenHermes-2-Mistral-7B", 0.2m },
        { "Austism/chronos-hermes-13b", 0.3m },
        { "garage-bAInd/Platypus2-70B-instruct", 0.9m },
        { "Gryphe/MythoMax-L2-13b", 0.3m },
        { "togethercomputer/alpaca-7b", 0.2m },
        { "WizardLM/WizardLM-13B-V1.2", 0.2m },
        { "upstage/SOLAR-10.7B-Instruct-v1.0", 0.3m },
        { "OpenAssistant/llama2-70b-oasst-sft-v10", 0.9m },
        { "openchat/openchat-3.5-1210", 0.2m },
        { "Qwen/Qwen1.5-7B-Chat", 0.2m },
        { "Qwen/Qwen1.5-14B-Chat", 0.3m },
        { "Qwen/Qwen1.5-1.8B-Chat", 0.1m },
        { "cognitivecomputations/dolphin-2.5-mixtral-8x7b", 0.6m },
        { "mistralai/Mixtral-8x22B-Instruct-v0.1", 1.2m },
        { "lmsys/vicuna-13b-v1.5", 0.3m },
        { "Qwen/Qwen1.5-0.5B-Chat", 0.1m },
        { "Qwen/Qwen1.5-4B-Chat", 0.1m },
        { "mistralai/Mistral-7B-Instruct-v0.1", 0.2m },
        { "mistralai/Mistral-7B-Instruct-v0.2", 0.2m },
        { "togethercomputer/Pythia-Chat-Base-7B", 0.2m },
        { "Qwen/Qwen1.5-32B-Chat", 0.8m },
        { "Qwen/Qwen1.5-72B-Chat", 0.9m },
        { "mistralai/Mistral-7B-Instruct-v0.3", 0.2m },
        { "Qwen/Qwen1.5-110B-Chat", 1.8m },
        { "mistralai/Mixtral-8x7B-Instruct-v0.1", 0.6m },
    };
    
    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request, 
        CancellationToken cancellationToken)
    {
        if (!request.AppProviderAttrs.TryGetValue("apiKey", out var apiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        var client = httpClientFactory.CreateClient(ProviderIds.TogetherAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var togetherAiInput = inputMapper.Map(request.Input) as TogetherAiCompletionInput;
        if (togetherAiInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        
        if (!string.IsNullOrWhiteSpace(request.Model))
            togetherAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            togetherAiInput.Messages.Insert(0, new TogetherAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            togetherAiInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            togetherAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProviderAttrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            togetherAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProviderAttrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            togetherAiInput.PresencePenalty = presencePenalty;
        }
        
        var response = await client.PostAsJsonAsync("chat/completions", togetherAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return new CompletionResponse
            {
                StatusCode = (int)response.StatusCode,
                Error = responseBody,
            };
        }

        var responseOutput = JsonSerializer.Deserialize<TogetherAiCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
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
            InputCost = CalculateInputCost(responseOutput.Model, usage.PromptTokens),
            OutputCost = CalculateOutputCost(responseOutput.Model, usage.CompletionTokens)
        };

        return completionResponse;
    }
    
    private static decimal CalculateInputCost(
        string model,
        int tokens)
    {
        if (ModelInputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }

    private static decimal CalculateOutputCost(
        string model,
        int tokens)
    {
        if (ModelOutputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }
}
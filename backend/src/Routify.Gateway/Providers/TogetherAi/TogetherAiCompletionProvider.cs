using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionProvider(
    IHttpClientFactory httpClientFactory) 
    : CompletionProviderBase<TogetherAiCompletionInput, TogetherAiCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "Qwen/Qwen2-72B-Instruct", new CompletionModel
            {
                Id = "Qwen/Qwen2-72B-Instruct",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "meta-llama/Llama-3-70b-chat-hf", new CompletionModel
            {
                Id = "meta-llama/Llama-3-70b-chat-hf",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "Snowflake/snowflake-arctic-instruct", new CompletionModel
            {
                Id = "Snowflake/snowflake-arctic-instruct",
                InputCost = 2.4m,
                OutputCost = 2.4m
            }
        },
        {
            "meta-llama/Llama-3-8b-chat-hf", new CompletionModel
            {
                Id = "meta-llama/Llama-3-8b-chat-hf",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "microsoft/WizardLM-2-8x22B", new CompletionModel
            {
                Id = "microsoft/WizardLM-2-8x22B",
                InputCost = 1.2m,
                OutputCost = 1.2m
            }
        },
        {
            "togethercomputer/StripedHyena-Nous-7B", new CompletionModel
            {
                Id = "togethercomputer/StripedHyena-Nous-7B",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "databricks/dbrx-instruct", new CompletionModel
            {
                Id = "databricks/dbrx-instruct",
                InputCost = 1.2m,
                OutputCost = 1.2m
            }
        },
        {
            "allenai/OLMo-7B-Instruct", new CompletionModel
            {
                Id = "allenai/OLMo-7B-Instruct",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "deepseek-ai/deepseek-llm-67b-chat", new CompletionModel
            {
                Id = "deepseek-ai/deepseek-llm-67b-chat",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "google/gemma-7b-it", new CompletionModel
            {
                Id = "google/gemma-7b-it",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "google/gemma-2b-it", new CompletionModel
            {
                Id = "google/gemma-2b-it",
                InputCost = 0.1m,
                OutputCost = 0.1m
            }
        },
        {
            "NousResearch/Nous-Hermes-2-Mistral-7B-DPO", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-2-Mistral-7B-DPO",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-2-Mixtral-8x7B-SFT",
                InputCost = 0.6m,
                OutputCost = 0.6m
            }
        },
        {
            "NousResearch/Nous-Hermes-2-Yi-34B", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-2-Yi-34B",
                InputCost = 0.8m,
                OutputCost = 0.8m
            }
        },
        {
            "codellama/CodeLlama-70b-Instruct-hf", new CompletionModel
            {
                Id = "codellama/CodeLlama-70b-Instruct-hf",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO",
                InputCost = 0.6m,
                OutputCost = 0.6m
            }
        },
        {
            "snorkelai/Snorkel-Mistral-PairRM-DPO", new CompletionModel
            {
                Id = "snorkelai/Snorkel-Mistral-PairRM-DPO",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "deepseek-ai/deepseek-coder-33b-instruct", new CompletionModel
            {
                Id = "deepseek-ai/deepseek-coder-33b-instruct",
                InputCost = 0.8m,
                OutputCost = 0.8m
            }
        },
        {
            "zero-one-ai/Yi-34B-Chat", new CompletionModel
            {
                Id = "zero-one-ai/Yi-34B-Chat",
                InputCost = 0.8m,
                OutputCost = 0.8m
            }
        },
        {
            "NousResearch/Nous-Hermes-Llama2-13b", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-Llama2-13b",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "NousResearch/Nous-Hermes-llama-2-7b", new CompletionModel
            {
                Id = "NousResearch/Nous-Hermes-llama-2-7b",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "togethercomputer/Llama-2-7B-32K-Instruct", new CompletionModel
            {
                Id = "togethercomputer/Llama-2-7B-32K-Instruct",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "meta-llama/Llama-2-70b-chat-hf", new CompletionModel
            {
                Id = "meta-llama/Llama-2-70b-chat-hf",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "meta-llama/Llama-2-13b-chat-hf", new CompletionModel
            {
                Id = "meta-llama/Llama-2-13b-chat-hf",
                InputCost = 0.22m,
                OutputCost = 0.22m
            }
        },
        {
            "meta-llama/Llama-2-7b-chat-hf", new CompletionModel
            {
                Id = "meta-llama/Llama-2-7b-chat-hf",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "codellama/CodeLlama-13b-Instruct-hf", new CompletionModel
            {
                Id = "codellama/CodeLlama-13b-Instruct-hf",
                InputCost = 0.22m,
                OutputCost = 0.22m
            }
        },
        {
            "codellama/CodeLlama-34b-Instruct-hf", new CompletionModel
            {
                Id = "codellama/CodeLlama-34b-Instruct-hf",
                InputCost = 0.78m,
                OutputCost = 0.78m
            }
        },
        {
            "codellama/CodeLlama-7b-Instruct-hf", new CompletionModel
            {
                Id = "codellama/CodeLlama-7b-Instruct-hf",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "NousResearch/Nous-Capybara-7B-V1p9", new CompletionModel
            {
                Id = "NousResearch/Nous-Capybara-7B-V1p9",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "teknium/OpenHermes-2p5-Mistral-7B", new CompletionModel
            {
                Id = "teknium/OpenHermes-2p5-Mistral-7B",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Open-Orca/Mistral-7B-OpenOrca", new CompletionModel
            {
                Id = "Open-Orca/Mistral-7B-OpenOrca",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "teknium/OpenHermes-2-Mistral-7B", new CompletionModel
            {
                Id = "teknium/OpenHermes-2-Mistral-7B",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Austism/chronos-hermes-13b", new CompletionModel
            {
                Id = "Austism/chronos-hermes-13b",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "garage-bAInd/Platypus2-70B-instruct", new CompletionModel
            {
                Id = "garage-bAInd/Platypus2-70B-instruct",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "Gryphe/MythoMax-L2-13b", new CompletionModel
            {
                Id = "Gryphe/MythoMax-L2-13b",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "togethercomputer/alpaca-7b", new CompletionModel
            {
                Id = "togethercomputer/alpaca-7b",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "WizardLM/WizardLM-13B-V1.2", new CompletionModel
            {
                Id = "WizardLM/WizardLM-13B-V1.2",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "upstage/SOLAR-10.7B-Instruct-v1.0", new CompletionModel
            {
                Id = "upstage/SOLAR-10.7B-Instruct-v1.0",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "OpenAssistant/llama2-70b-oasst-sft-v10", new CompletionModel
            {
                Id = "OpenAssistant/llama2-70b-oasst-sft-v10",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "openchat/openchat-3.5-1210", new CompletionModel
            {
                Id = "openchat/openchat-3.5-1210",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Qwen/Qwen1.5-7B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-7B-Chat",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Qwen/Qwen1.5-14B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-14B-Chat",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "Qwen/Qwen1.5-1.8B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-1.8B-Chat",
                InputCost = 0.1m,
                OutputCost = 0.1m
            }
        },
        {
            "cognitivecomputations/dolphin-2.5-mixtral-8x7b", new CompletionModel
            {
                Id = "cognitivecomputations/dolphin-2.5-mixtral-8x7b",
                InputCost = 0.6m,
                OutputCost = 0.6m
            }
        },
        {
            "mistralai/Mixtral-8x22B-Instruct-v0.1", new CompletionModel
            {
                Id = "mistralai/Mixtral-8x22B-Instruct-v0.1",
                InputCost = 1.2m,
                OutputCost = 1.2m
            }
        },
        {
            "lmsys/vicuna-13b-v1.5", new CompletionModel
            {
                Id = "lmsys/vicuna-13b-v1.5",
                InputCost = 0.3m,
                OutputCost = 0.3m
            }
        },
        {
            "Qwen/Qwen1.5-0.5B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-0.5B-Chat",
                InputCost = 0.1m,
                OutputCost = 0.1m
            }
        },
        {
            "Qwen/Qwen1.5-4B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-4B-Chat",
                InputCost = 0.1m,
                OutputCost = 0.1m
            }
        },
        {
            "mistralai/Mistral-7B-Instruct-v0.1", new CompletionModel
            {
                Id = "mistralai/Mistral-7B-Instruct-v0.1",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "mistralai/Mistral-7B-Instruct-v0.2", new CompletionModel
            {
                Id = "mistralai/Mistral-7B-Instruct-v0.2",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "togethercomputer/Pythia-Chat-Base-7B", new CompletionModel
            {
                Id = "togethercomputer/Pythia-Chat-Base-7B",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Qwen/Qwen1.5-32B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-32B-Chat",
                InputCost = 0.8m,
                OutputCost = 0.8m
            }
        },
        {
            "Qwen/Qwen1.5-72B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-72B-Chat",
                InputCost = 0.9m,
                OutputCost = 0.9m
            }
        },
        {
            "mistralai/Mistral-7B-Instruct-v0.3", new CompletionModel
            {
                Id = "mistralai/Mistral-7B-Instruct-v0.3",
                InputCost = 0.2m,
                OutputCost = 0.2m
            }
        },
        {
            "Qwen/Qwen1.5-110B-Chat", new CompletionModel
            {
                Id = "Qwen/Qwen1.5-110B-Chat",
                InputCost = 1.8m,
                OutputCost = 1.8m
            }
        },
        {
            "mistralai/Mixtral-8x7B-Instruct-v0.1", new CompletionModel
            {
                Id = "mistralai/Mixtral-8x7B-Instruct-v0.1",
                InputCost = 0.6m,
                OutputCost = 0.6m
            }
        },
    };
    public override string Id => ProviderIds.TogetherAi;
    
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
        TogetherAiCompletionInput input, 
        TogetherAiCompletionOutput output)
    {
        return output.Model;
    }
    
    protected override int GetInputTokens(
        TogetherAiCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }
    
    protected override int GetOutputTokens(
        TogetherAiCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }

    protected override TogetherAiCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var togetherAiInput = TogetherAiCompletionInputMapper.Map(request.Input);
        
        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            togetherAiInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            togetherAiInput.Messages.Insert(0, new TogetherAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            togetherAiInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            togetherAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            togetherAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            togetherAiInput.PresencePenalty = presencePenalty;
        }

        return togetherAiInput;
    }
    
    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<TogetherAiCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var openAiOutput = TogetherAiCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(openAiOutput);
    }
}
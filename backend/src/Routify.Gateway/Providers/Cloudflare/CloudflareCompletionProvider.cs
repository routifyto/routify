using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Providers.Cloudflare.Models;

namespace Routify.Gateway.Providers.Cloudflare;

internal class CloudflareCompletionProvider(
    IHttpClientFactory httpClientFactory)
    : CompletionProviderBase<CloudflareCompletionInput, CloudflareCompletionOutput>
{
    private static readonly Dictionary<string, CompletionModel> _models = new()
    {
        {
            "llama-2-7b-chat-fp16", new CompletionModel
            {
                Id = "llama-2-7b-chat-fp16",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "mistral-7b-instruct-v0.1", new CompletionModel
            {
                Id = "mistral-7b-instruct-v0.1",
                InputCost = 0m,
                OutputCost = 0m
            }
        },

        {
            "deepseek-coder-6.7b-base-awq", new CompletionModel
            {
                Id = "deepseek-coder-6.7b-base-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "deepseek-coder-6.7b-instruct-awq", new CompletionModel
            {
                Id = "deepseek-coder-6.7b-instruct-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "deepseek-math-7b-base", new CompletionModel
            {
                Id = "deepseek-math-7b-base",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "deepseek-math-7b-instruct", new CompletionModel
            {
                Id = "deepseek-math-7b-instruct",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "discolm-german-7b-v1-awq", new CompletionModel
            {
                Id = "discolm-german-7b-v1-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "falcon-7b-instruct", new CompletionModel
            {
                Id = "falcon-7b-instruct",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "gemma-2b-it-lora", new CompletionModel
            {
                Id = "gemma-2b-it-lora",
                InputCost = 0m,
                OutputCost = 0m
            }
        },

        {
            "gemma-7b-it", new CompletionModel
            {
                Id = "gemma-7b-it",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "gemma-7b-it-lora", new CompletionModel
            {
                Id = "gemma-7b-it-lora",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "hermes-2-pro-mistral-7b", new CompletionModel
            {
                Id = "hermes-2-pro-mistral-7b",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "llama-2-13b-chat-awq", new CompletionModel
            {
                Id = "llama-2-13b-chat-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "llama-2-7b-chat-hf-lora", new CompletionModel
            {
                Id = "llama-2-7b-chat-hf-lora",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "llama-3-8b-instruct", new CompletionModel
            {
                Id = "llama-3-8b-instruct",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "llama-3-8b-instruct-awq", new CompletionModel
            {
                Id = "llama-3-8b-instruct-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "llamaguard-7b-awq", new CompletionModel
            {
                Id = "llamaguard-7b-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "mistral-7b-instruct-v0.1-awq", new CompletionModel
            {
                Id = "mistral-7b-instruct-v0.1-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "mistral-7b-instruct-v0.2", new CompletionModel
            {
                Id = "mistral-7b-instruct-v0.2",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "mistral-7b-instruct-v0.2-lora", new CompletionModel
            {
                Id = "mistral-7b-instruct-v0.2-lora",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "neural-chat-7b-v3-1-awq", new CompletionModel
            {
                Id = "neural-chat-7b-v3-1-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "openchat-3.5-0106", new CompletionModel
            {
                Id = "openchat-3.5-0106",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "openhermes-2.5-mistral-7b-awq", new CompletionModel
            {
                Id = "openhermes-2.5-mistral-7b-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "phi-2", new CompletionModel
            {
                Id = "phi-2",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "qwen1.5-0.5b-chat", new CompletionModel
            {
                Id = "qwen1.5-0.5b-chat",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "qwen1.5-1.8b-chat", new CompletionModel
            {
                Id = "qwen1.5-1.8b-chat",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "qwen1.5-14b-chat-awq", new CompletionModel
            {
                Id = "qwen1.5-14b-chat-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "qwen1.5-7b-chat-awq", new CompletionModel
            {
                Id = "qwen1.5-7b-chat-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "sqlcoder-7b-2", new CompletionModel
            {
                Id = "sqlcoder-7b-2",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "starling-lm-7b-beta", new CompletionModel
            {
                Id = "starling-lm-7b-beta",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "tinyllama-1.1b-chat-v1.0", new CompletionModel
            {
                Id = "tinyllama-1.1b-chat-v1.0",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "una-cybertron-7b-v2-bf16", new CompletionModel
            {
                Id = "una-cybertron-7b-v2-bf16",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
        {
            "zephyr-7b-beta-awq", new CompletionModel
            {
                Id = "zephyr-7b-beta-awq",
                InputCost = 0m,
                OutputCost = 0m
            }
        },
    };

    public override string Id => ProviderIds.Cloudflare;
    public override Dictionary<string, CompletionModel> Models => _models;

    protected override HttpClient PrepareHttpClient(
        CompletionRequest request)
    {
        var apiKey = request.AppProvider.Attrs.GetValueOrDefault("apiToken");
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new GatewayException(HttpStatusCode.Unauthorized);

        var client = httpClientFactory.CreateClient(Id);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        return client;
    }

    protected override string PrepareRequestUrl(
        CompletionRequest request)
    {
        if (!request.AppProvider.Attrs.TryGetValue("accountId", out var accountId))
            throw new GatewayException(HttpStatusCode.BadRequest);

        return $"{accountId}/ai/v1/chat/completions";
    }

    protected override string GetModel(
        CloudflareCompletionInput input, 
        CloudflareCompletionOutput output)
    {
        return output.Model;
    }

    protected override int GetInputTokens(
        CloudflareCompletionOutput output)
    {
        return output.Usage.PromptTokens;
    }

    protected override int GetOutputTokens(
        CloudflareCompletionOutput output)
    {
        return output.Usage.CompletionTokens;
    }

    protected override CloudflareCompletionInput PrepareInput(
        CompletionRequest request)
    {
        var cloudflareInput = CloudflareCompletionInputMapper.Map(request.Input);

        if (!string.IsNullOrWhiteSpace(request.RouteProvider.Model))
            cloudflareInput.Model = request.RouteProvider.Model;

        if (request.RouteProvider.Attrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            cloudflareInput.Messages.Insert(0, new CloudflareCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProvider.Attrs.TryGetValue("temperature", out var temperatureString)
            && !string.IsNullOrWhiteSpace(temperatureString)
            && float.TryParse(temperatureString, out var temperature))
        {
            cloudflareInput.Temperature = temperature;
        }

        if (request.RouteProvider.Attrs.TryGetValue("maxTokens", out var maxTokensString)
            && !string.IsNullOrWhiteSpace(maxTokensString)
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            cloudflareInput.MaxTokens = maxTokens;
        }

        if (request.RouteProvider.Attrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString)
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString)
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            cloudflareInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProvider.Attrs.TryGetValue("presencePenalty", out var presencePenaltyString)
            && !string.IsNullOrWhiteSpace(presencePenaltyString)
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            cloudflareInput.PresencePenalty = presencePenalty;
        }

        return cloudflareInput;
    }

    public override ICompletionInput? ParseInput(
        string input)
    {
        return RoutifyJsonSerializer.Deserialize<CloudflareCompletionInput>(input);
    }

    public override string SerializeOutput(
        ICompletionOutput output)
    {
        var cloudflareOutput = CloudflareCompletionOutputMapper.Map(output);
        return RoutifyJsonSerializer.Serialize(cloudflareOutput);
    }
}
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Cohere.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionInputMapper
{
    public static CohereCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            CohereCompletionInput cohereCompletionInput => cohereCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralCompletionInput mistralCompletionInput => MapMistralCompletionInput(mistralCompletionInput),
            GroqCompletionInput groqCompletionInput => MapGroqCompletionInput(groqCompletionInput),
            CloudflareCompletionInput cloudflareCompletionInput => MapCloudflareCompletionInput(cloudflareCompletionInput),
            PerplexityCompletionInput perplexityCompletionInput => MapPerplexityCompletionInput(perplexityCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static CohereCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
            Seed = input.Seed
        };
    }
    
    private static CohereCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            K = input.TopK,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
        };
    }
    
    private static CohereCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            chatHistory.Insert(0, new CohereCompletionMessageInput
            {
                Message = input.System,
                Role = "SYSTEM"
            });
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            K = input.TopK,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
        };
    }
    
    private static CohereCompletionInput MapMistralCompletionInput(
        MistralCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Seed = input.RandomSeed,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
        };
    }
    
    private static CohereCompletionInput MapGroqCompletionInput(
        GroqCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
            Seed = input.Seed
        };
    }
    
    private static CohereCompletionInput MapCloudflareCompletionInput(
        CloudflareCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
            Seed = input.Seed
        };
    }
    
    private static CohereCompletionInput MapPerplexityCompletionInput(
        PerplexityCompletionInput input)
    {
        var lastMessage = input.Messages.LastOrDefault();
        var chatHistory = new List<CohereCompletionMessageInput>();
        if (input.Messages.Count > 1)
        {
            var otherMessages = input.Messages.Take(input.Messages.Count - 1);
            var cohereMessages = otherMessages
                .Select(message => new CohereCompletionMessageInput
                {
                    Message = message.Content,
                    Role = MapCohereRole(message.Role)
                });
            
            chatHistory.AddRange(cohereMessages);
        }
        
        return new CohereCompletionInput
        {
            Model = input.Model,
            P = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            ChatHistory = chatHistory,
            Message = lastMessage?.Content,
        };
    }

    private static string MapCohereRole(
        string role)
    {
        if (role.Equals("assistant", StringComparison.InvariantCultureIgnoreCase))
        {
            return "CHATBOT";
        }

        return role.ToUpperInvariant();
    }
}
using System.Text.Json;
using Routify.Provider.Core.Completion;
using Routify.Provider.OpenAi.Models;

namespace Routify.Provider.OpenAi;

internal class OpenAiCompletionInputParser : ICompletionInputParser
{
    public CompletionInput? Parse(
        string input)
    {
        var openAiInput = JsonSerializer.Deserialize<OpenAiCompletionInput>(input);
        if (openAiInput == null)
            return null;

        return new CompletionInput
        {
            Model = openAiInput.Model,
            Messages = openAiInput
                .Messages
                .Select(message => new CompletionMessageInput
                {
                    Content = message.Content,
                    Name = message.Name,
                    Role = message.Role
                })
                .ToList(),
            TopP = openAiInput.TopP,
            N = openAiInput.N,
            Stop = openAiInput.Stop,
            MaxTokens = openAiInput.MaxTokens,

            PresencePenalty = openAiInput.PresencePenalty,
            FrequencyPenalty = openAiInput.FrequencyPenalty,
            Temperature = openAiInput.Temperature
        };
    }
}
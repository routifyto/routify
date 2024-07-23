namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageContentInput
{
    public string? StringValue { get; set; }
    public List<AnthropicCompletionMessageContentBlockInput>? ListValue { get; set; }
}
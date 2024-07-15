namespace Routify.Gateway.Abstractions;

internal record CompletionModel
{
    public string Id { get; init; } = null!;
    public decimal InputCost { get; init; }
    public decimal OutputCost { get; init; }

    public int InputCostUnit { get; init; } = 1_000_000;
    public int OutputCostUnit { get; init; } = 1_000_000;
}
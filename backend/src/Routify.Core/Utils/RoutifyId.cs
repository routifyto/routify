using NUlid.Rng;

namespace Routify.Core.Utils;

public static class RoutifyId
{
    private static readonly IUlidRng Rng = new MonotonicUlidRng();

    public static string Ulid()
    {
        return NUlid.Ulid.NewUlid(Rng).ToString();
    }

    public static string Generate(
        string type)
    {
        return NUlid.Ulid.NewUlid(Rng).ToString().ToLowerInvariant() + type;
    }

    public static bool Is(
        string id, 
        string type)
    {
        return !string.IsNullOrWhiteSpace(id) && id.EndsWith(type);
    }

    public static string GetType(
        string id)
    {
        return string.IsNullOrWhiteSpace(id) ? id : id[^2..];
    }
    
    public static DateTime GetTimestamp(
        string id)
    {
        var idOnly = id[..^2];
        var upperCase = idOnly.ToUpperInvariant();
        return NUlid.Ulid.Parse(upperCase).Time.DateTime;
    }

    private static int Length => 28;
}

public static class IdType
{
    public const string App = "ap";
    public const string AppUser = "au";
    public const string Route = "rt";
    public const string RouteModel = "rm";
    public const string User = "us";
    public const string Version = "ve";
    public const string AppProvider = "pr";
    public const string CompletionLog = "cl";
    public const string ApiKey = "ak";
    public const string Consumer = "cn";
}
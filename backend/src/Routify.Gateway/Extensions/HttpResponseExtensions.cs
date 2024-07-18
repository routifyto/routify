namespace Routify.Gateway.Extensions;

internal static class HttpResponseExtensions
{
    public static Dictionary<string, string> GetHeaders(
        this HttpResponse response)
    {
        return new Dictionary<string, string>();
        // return response
        //     .Headers
        //     .ToDictionary(x => x.Key, x => x.Value.ToString());
    }
}
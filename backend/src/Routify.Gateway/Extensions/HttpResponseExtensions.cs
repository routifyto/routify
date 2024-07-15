using Routify.Data.Common;

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

    public static ResponseLog ToResponseLog(
        this HttpResponseMessage responseMessage,
        string? body)
    {
        // var headers = responseMessage
        //     .Headers
        //     .ToDictionary(x => x.Key, x => string.Join(",", x.Value));

        return new ResponseLog
        {
            StatusCode = (int)responseMessage.StatusCode,
            Headers = new Dictionary<string, string>(),
            Body = body
        };
    }
}
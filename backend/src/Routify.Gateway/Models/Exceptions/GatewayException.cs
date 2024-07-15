using System.Net;

namespace Routify.Gateway.Models.Exceptions;

internal class GatewayException(
    HttpStatusCode statusCode,
    string? body = null) : Exception
{
    public HttpStatusCode StatusCode => statusCode;
    public string? Body => body;
}
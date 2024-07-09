namespace Routify.Gateway.Abstractions;

internal interface IRequestHandler
{
    Task HandleAsync(RequestContext context);
}
using System.Net;
using Routify.Core.Constants;
using Routify.Core.Utils;
using Routify.Data.Common;
using Routify.Data.Enums;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Models.Data;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Services;
using RouteData = Routify.Gateway.Models.Data.RouteData;

namespace Routify.Gateway.Handlers;

internal class CompletionHandler(
    IServiceProvider serviceProvider,
    LogService logService)
    : IRequestHandler
{
    private readonly Random _random = new();
    
    public async Task HandleAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        var log = new CompletionLog
        {
            Id = RoutifyId.Generate(IdType.CompletionLog),
            AppId = context.App.Id,
            RouteId = context.Route.Id,
            Path = context.Route.Path,
            StartedAt = DateTime.UtcNow,
            ApiKeyId = context.ApiKey.Id,
            ConsumerId = context.Consumer?.Id,
            GatewayRequest = context.HttpContext.Request.ToRequestLog(),
        };

        try
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;

            var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);
            log.GatewayRequest.Body = requestBody;

            var schemaProvider = serviceProvider.GetKeyedService<ICompletionProvider>(context.Route.Schema);
            if (schemaProvider == null)
                throw new GatewayException(HttpStatusCode.NotImplemented);

            var input = schemaProvider.ParseInput(requestBody);
            if (input == null)
                throw new GatewayException(HttpStatusCode.BadRequest);

            var routeProvider = ChooseRouteProvider(context.Route);
            if (routeProvider == null)
                throw new GatewayException(HttpStatusCode.NotImplemented);

            var appProvider = context.App.GetProviderById(routeProvider.AppProviderId);
            if (appProvider == null)
                throw new GatewayException(HttpStatusCode.NotImplemented);

            var completionProvider = serviceProvider.GetKeyedService<ICompletionProvider>(appProvider.Provider);
            if (completionProvider == null)
                throw new GatewayException(HttpStatusCode.NotImplemented);

            log.AppProviderId = appProvider.Id;
            log.RouteProviderId = routeProvider.Id;
            log.Provider = appProvider.Provider;
            
            var completionRequest = new CompletionRequest
            {
                Input = input,
                AppProvider = appProvider,
                RouteProvider = routeProvider,
            };

            var completionResponse = await completionProvider.CompleteAsync(completionRequest, cancellationToken);
            var responseBody = string.Empty;
            if (completionResponse.Output != null)
            {
                responseBody = schemaProvider.SerializeOutput(completionResponse.Output);
            }
            else if (string.IsNullOrWhiteSpace(completionResponse.Error))
            {
                responseBody = completionResponse.Error;
            }
            
            log.Model = completionResponse.Model;
            log.InputTokens = completionResponse.InputTokens;
            log.OutputTokens = completionResponse.OutputTokens;
            log.InputCost = completionResponse.InputCost;
            log.OutputCost = completionResponse.OutputCost;
            log.ProviderRequest = completionResponse.RequestLog;
            log.ProviderResponse = completionResponse.ResponseLog;
            
            log.GatewayResponse = new ResponseLog
            {
                StatusCode = completionResponse.StatusCode,
                Body = responseBody ?? string.Empty,
                Headers = context.HttpContext.Response.GetHeaders()
            };
            
            context.HttpContext.Response.StatusCode = completionResponse.StatusCode;
            context.HttpContext.Response.ContentType = "application/json";
            await context.HttpContext.Response.WriteAsync(responseBody ?? string.Empty, cancellationToken);
        }
        catch (GatewayException ex)
        {
            context.HttpContext.Response.StatusCode = (int)ex.StatusCode;
            if (!string.IsNullOrWhiteSpace(ex.Body))
            {
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(ex.Body, cancellationToken);
            }
        }
        catch (Exception)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        finally
        {
            log.EndedAt = DateTime.UtcNow;
            log.Duration = (log.EndedAt - log.StartedAt).TotalMilliseconds;
            logService.Save(log);
        }
    }
    
    private RouteProviderData? ChooseRouteProvider(
        RouteData route)
    {
        if (route.Strategy == RouteStrategy.LoadBalance)
        {
            var totalWeight = route.TotalWeight;
            var randomWeight = _random.Next(0, totalWeight);
            
            var routeProviders = route.Providers
                .FirstOrDefault(x => randomWeight >= x.WeightFrom && randomWeight < x.WeightTo);
            
            if (routeProviders != null)
                return routeProviders;
        }

        return route.Providers.FirstOrDefault();
    }
}
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Routify.Core.Utils;
using Routify.Data.Enums;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Extensions;
using Routify.Gateway.Models.Exceptions;
using Routify.Gateway.Services;
using Routify.Gateway.Utils;

namespace Routify.Gateway.Handlers;

internal class CompletionHandler(
    IServiceProvider serviceProvider,
    LogService logService,
    CostService costService)
    : IRequestHandler
{
    public async Task HandleAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        var request = context.HttpContext.Request;
        var log = new CompletionLog
        {
            Id = RoutifyId.Generate(IdType.CompletionLog),
            AppId = context.App.Id,
            RouteId = context.Route.Id,
            Path = context.Route.Path,
            StartedAt = DateTime.UtcNow,
            ApiKeyId = context.ApiKey.Id,
            ConsumerId = context.Consumer?.Id,
            RequestUrl = request.GetDisplayUrl(),
            RequestMethod = request.Method,
        };

        var outgoingLogs = new List<CompletionOutgoingLog>();
        try
        {
            var schemaProvider = serviceProvider.GetKeyedService<ICompletionProvider>(context.Route.Schema);
            if (schemaProvider == null)
                throw new GatewayException(HttpStatusCode.NotImplemented);

            var requestBody = await request.ReadBodyAsync(cancellationToken) ?? string.Empty;
            log.RequestBody = requestBody;

            var input = schemaProvider.ParseInput(requestBody);
            if (input == null)
                throw new GatewayException(HttpStatusCode.BadRequest);

            var isCostLimitEnabled = context.Route.CostLimitConfig?.Enabled == true;
            if (isCostLimitEnabled && context.Route.CostLimitConfig != null)
            {
                var hasReachedCostLimit = await costService.HasReachedCostLimit(context.Route.Id, context.Route.CostLimitConfig);
                if (hasReachedCostLimit)
                    throw new GatewayException(HttpStatusCode.TooManyRequests);
            }
            
            var routeProviderSelector = new RouteProviderSelector(context.Route);
            var isDone = false;

            while (routeProviderSelector.HasNextProvider && !isDone)
            {
                var routeProvider = routeProviderSelector.GetNextProvider();
                if (routeProvider == null)
                    throw new GatewayException(HttpStatusCode.NotImplemented);

                var appProvider = context.App.GetProviderById(routeProvider.AppProviderId);
                if (appProvider == null)
                    throw new GatewayException(HttpStatusCode.NotImplemented);

                var completionProvider = serviceProvider.GetKeyedService<ICompletionProvider>(appProvider.Provider);
                if (completionProvider == null)
                    throw new GatewayException(HttpStatusCode.NotImplemented);

                var completionRequest = new CompletionRequest
                {
                    Context = context,
                    LogId = log.Id,
                    Input = input,
                    AppProvider = appProvider,
                    RouteProvider = routeProvider,
                };

                var completionResponse = await completionProvider.CompleteAsync(completionRequest, cancellationToken);
                
                if (completionResponse.Log != null)
                    outgoingLogs.Add(completionResponse.Log);
                
                if (context.Route.IsFailoverEnabled
                    && HttpUtils.ShouldRetry(completionResponse.StatusCode)
                    && routeProviderSelector.HasNextProvider)
                {
                    continue;
                }

                var responseBody = string.Empty;
                if (completionResponse.Output != null)
                {
                    responseBody = schemaProvider.SerializeOutput(completionResponse.Output);
                }
                else if (!string.IsNullOrWhiteSpace(completionResponse.Error))
                {
                    responseBody = completionResponse.Error;
                }

                log.AppProviderId = appProvider.Id;
                log.RouteProviderId = routeProvider.Id;
                log.Provider = appProvider.Provider;
                log.Model = completionResponse.Model;
                log.InputTokens = completionResponse.InputTokens;
                log.OutputTokens = completionResponse.OutputTokens;
                log.InputCost = completionResponse.InputCost;
                log.OutputCost = completionResponse.OutputCost;
                log.StatusCode = completionResponse.StatusCode;
                log.ResponseBody = responseBody;
                log.OutgoingRequestsCount = outgoingLogs.Count;
                log.CacheStatus = completionResponse.CacheStatus;

                context.HttpContext.Response.StatusCode = completionResponse.StatusCode;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(responseBody ?? string.Empty, cancellationToken);
                isDone = true;

                if (isCostLimitEnabled && log.CacheStatus != CacheStatus.Hit)
                {
                    var totalCost = completionResponse.InputCost + completionResponse.OutputCost;
                    await serviceProvider.GetRequiredService<CostService>().SaveCost(context.Route.Id, totalCost);
                }
            }

            if (!isDone)
                throw new GatewayException(HttpStatusCode.NotImplemented);
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
#if DEBUG
            throw;
#endif
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        finally
        {
            log.EndedAt = DateTime.UtcNow;
            log.Duration = (log.EndedAt - log.StartedAt).TotalMilliseconds;

            logService.SaveCompletionLog(log);
            logService.SaveCompletionOutgoingLogs(outgoingLogs);
        }
    }
}
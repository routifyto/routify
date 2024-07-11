using Routify.Core.Utils;
using Routify.Gateway.Models.Api;

namespace Routify.Gateway.Services;

internal class Synchronizer(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    Repository repository)
    : BackgroundService
{
    protected override Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var syncPeriodSeconds = configuration.GetValue("Api:SyncPeriod", 5);
        return Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    var client = httpClientFactory.CreateClient("api");
                    var payloadResponse = await client.GetStringAsync("/v1/gateway/data", stoppingToken);
                    if (string.IsNullOrWhiteSpace(payloadResponse))
                        continue;

                    var data = RoutifyJsonSerializer.Deserialize<ApiDataPayload>(payloadResponse);
                    if (data == null)
                        continue;

                    repository.UpdateApps(data.Apps);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(TimeSpan.FromSeconds(syncPeriodSeconds), stoppingToken);
            }
        }, stoppingToken);
    }
}
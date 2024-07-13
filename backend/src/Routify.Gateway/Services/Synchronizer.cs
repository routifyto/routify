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
                    var outputResponse = await client.GetStringAsync("/v1/gateway/data", stoppingToken);
                    if (string.IsNullOrWhiteSpace(outputResponse))
                        continue;

                    var data = RoutifyJsonSerializer.Deserialize<ApiDataOutput>(outputResponse);
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
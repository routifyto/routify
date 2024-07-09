using System.Text.Json;
using System.Text.Json.Serialization;
using Routify.Gateway.Models.Api;

namespace Routify.Gateway.Services;

internal class Synchronizer(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    Repository repository)
    : BackgroundService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper)
        }
    };

    protected override Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var delaySeconds = configuration.GetValue("Api:SyncPeriod", 5);
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

                    var data = JsonSerializer.Deserialize<ApiDataPayload>(payloadResponse, Options);
                    if (data == null)
                        continue;

                    repository.UpdateApps(data.Apps);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
            }
        }, stoppingToken);
    }
}
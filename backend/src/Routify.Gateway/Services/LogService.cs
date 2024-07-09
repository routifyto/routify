using System.Collections.Concurrent;
using System.Text;
using Routify.Core.Utils;
using Routify.Data.Models;

namespace Routify.Gateway.Services;

internal class LogService(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory)
    : BackgroundService
{
    private readonly ConcurrentQueue<TextLog> _textLogs = new();
    
    public void Save(
        TextLog log)
    {
        _textLogs.Enqueue(log);
    }
    
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var writePeriodSeconds = configuration.GetValue("Api:WritePeriod", 5);
        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var logs = new List<TextLog>();
                    while (_textLogs.TryDequeue(out var log))
                    {
                        logs.Add(log);
                    }
                
                    if (logs.Count > 0)
                    {
                        var client = httpClientFactory.CreateClient("api");
                        var input = new LogsInput
                        {
                            TextLogs = logs
                        };
                        var inputJson = RoutifyJsonSerializer.Serialize(input);
                        var content = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        await client.PostAsync("/v1/gateway/logs", content, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(TimeSpan.FromSeconds(writePeriodSeconds), stoppingToken);
            }
        }, stoppingToken);
    }

    private record LogsInput
    {
        public List<TextLog> TextLogs { get; set; } = [];
    }
}
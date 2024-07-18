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
    private readonly ConcurrentQueue<CompletionLog> _completionLogs = new();
    private readonly ConcurrentQueue<CompletionOutgoingLog> _completionOutgoingLogs = new();
    
    public void SaveCompletionLog(
        CompletionLog log)
    {
        _completionLogs.Enqueue(log);
    }
    
    public void SaveCompletionOutgoingLogs(
        List<CompletionOutgoingLog> logs)
    {
        foreach (var log in logs)
        {
            _completionOutgoingLogs.Enqueue(log);
        }
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
                    var logs = new List<CompletionLog>();
                    while (_completionLogs.TryDequeue(out var log))
                    {
                        logs.Add(log);
                    }
                    
                    var completionOutgoingLogs = new List<CompletionOutgoingLog>();
                    while (_completionOutgoingLogs.TryDequeue(out var log))
                    {
                        completionOutgoingLogs.Add(log);
                    }
                
                    if (logs.Count > 0 || completionOutgoingLogs.Count > 0)
                    {
                        var client = httpClientFactory.CreateClient("api");
                        var input = new LogsInput
                        {
                            CompletionLogs = logs,
                            CompletionOutgoingLogs = completionOutgoingLogs
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
        public List<CompletionLog> CompletionLogs { get; set; } = [];
        public List<CompletionOutgoingLog> CompletionOutgoingLogs { get; set; } = [];
    }
}
using Routify.Data.Models;

namespace Routify.Api.Models.Gateway;

public class GatewayLogsInput
{
    public List<CompletionLog> CompletionLogs { get; set; } = [];
}
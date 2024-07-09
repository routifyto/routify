using Routify.Data.Models;

namespace Routify.Api.Models.Gateway;

public class GatewayLogsInput
{
    public List<TextLog> TextLogs { get; set; } = [];
}
using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Utils;

namespace Routify.Data.Models;

public record CompletionOutgoingLog
{
    public string Id { get; set; } = null!;
    public string IncomingLogId { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public string RouteProviderId { get; set; } = null!;
    public int RetryCount { get; set; }

    public string? RequestUrl { get; set; }
    public string? RequestMethod { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public string? RequestBody { get; set; }
    
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompletionOutgoingLog>(entity =>
        {
            entity.ToTable("routify_completion_outgoing_logs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.IncomingLogId)
                .HasColumnName("incoming_log_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.AppId)
                .HasColumnName("app_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.RouteId)
                .HasColumnName("route_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Provider)
                .HasColumnName("provider")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.AppProviderId)
                .HasColumnName("app_provider_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.RouteProviderId)
                .HasColumnName("route_provider_id")
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(e => e.RetryCount)
                .HasColumnName("retry_count");

            entity.Property(e => e.RequestUrl)
                .HasColumnName("request_url");

            entity.Property(e => e.RequestMethod)
                .HasColumnName("request_method")
                .HasMaxLength(10);

            entity.Property(e => e.RequestHeaders)
                .HasColumnName("request_headers")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>(),
                    ValueComparers.StringDictionary);

            entity.Property(e => e.RequestBody)
                .HasColumnName("request_body");

            entity.Property(e => e.StatusCode)
                .HasColumnName("status_code");

            entity.Property(e => e.ResponseBody)
                .HasColumnName("response_body");

            entity.Property(e => e.ResponseHeaders)
                .HasColumnName("response_headers")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>(),
                    ValueComparers.StringDictionary);

            entity.Property(e => e.StartedAt)
                .HasColumnName("started_at");

            entity.Property(e => e.EndedAt)
                .HasColumnName("ended_at");

            entity.Property(e => e.Duration)
                .HasColumnName("duration");
        });
    }
}
using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Enums;
using Routify.Data.Utils;

namespace Routify.Data.Models;

public record CompletionLog
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Provider { get; set; }
    public string? Model { get; set; }
    public string? AppProviderId { get; set; }
    public string? RouteProviderId { get; set; }
    public string ApiKeyId { get; set; } = null!;
    public string? ConsumerId { get; set; }
    public string? SessionId { get; set; }
    public int OutgoingRequestsCount { get; set; }
    public CacheStatus CacheStatus { get; set; }
    public string? RequestUrl { get; set; }
    public string? RequestMethod { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public string? RequestBody { get; set; }
    
    
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompletionLog>(entity =>
        {
            entity.ToTable("routify_completion_logs");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.AppId)
                .HasColumnName("app_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.RouteId)
                .HasColumnName("route_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Path)
                .HasColumnName("path")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Provider)
                .HasColumnName("provider")
                .HasMaxLength(30);

            entity.Property(e => e.Model)
                .HasColumnName("model")
                .HasMaxLength(100);

            entity.Property(e => e.AppProviderId)
                .HasColumnName("app_provider_id")
                .HasMaxLength(30);

            entity.Property(e => e.RouteProviderId)
                .HasColumnName("route_provider_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ApiKeyId)
                .HasColumnName("api_key_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.SessionId)
                .HasColumnName("session_id")
                .HasMaxLength(100);
            
            entity.Property(e => e.OutgoingRequestsCount)
                .HasColumnName("outgoing_requests_count");
            
            entity.Property(e => e.CacheStatus)
                .HasColumnName("cache_status")
                .IsRequired()
                .HasDefaultValue(CacheStatus.Disabled);
            
            entity.Property(e => e.ConsumerId)
                .HasColumnName("consumer_id")
                .HasMaxLength(100);

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
            
            entity.Property(e => e.InputTokens)
                .HasColumnName("input_tokens");

            entity.Property(e => e.OutputTokens)
                .HasColumnName("output_tokens");

            entity.Property(e => e.InputCost)
                .HasColumnName("input_cost");

            entity.Property(e => e.OutputCost)
                .HasColumnName("output_cost");

            entity.Property(e => e.StartedAt)
                .HasColumnName("started_at");

            entity.Property(e => e.EndedAt)
                .HasColumnName("ended_at");

            entity.Property(e => e.Duration)
                .HasColumnName("duration");
            
            entity.HasIndex(e => e.AppId);
        });
    }
}
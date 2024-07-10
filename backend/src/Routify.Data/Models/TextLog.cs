using Microsoft.EntityFrameworkCore;

namespace Routify.Data.Models;

public class TextLog
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Provider { get; set; }
    public string? Model { get; set; }
    public string? AppProviderId { get; set; }
    public string? RouteProviderId { get; set; } = null!;
    public string ApiKeyId { get; set; } = null!;
    public string? SessionId { get; set; }
    
    public string RequestBody { get; set; } = null!;
    
    public int ResponseStatusCode { get; set; }
    public string ResponseBody { get; set; } = null!;
    
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
        modelBuilder.Entity<TextLog>(entity =>
        {
            entity.ToTable("routify_text_logs");
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
                .HasMaxLength(30);

            entity.Property(e => e.RequestBody)
                .HasColumnName("request_body")
                .IsRequired();

            entity.Property(e => e.ResponseStatusCode)
                .HasColumnName("response_status_code");

            entity.Property(e => e.ResponseBody)
                .HasColumnName("response_body")
                .IsRequired();

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
        });
    }
}
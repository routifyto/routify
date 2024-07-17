using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Common;
using Routify.Data.Utils;

namespace Routify.Data.Models;

public record RouteProvider
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    
    public string? Model { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
    public RetryConfig? RetryConfig { get; set; }
    public int Weight { get; set; } = 1;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    public string VersionId { get; set; } = null!;
    
    public Route? Route { get; set; }
    public App? App { get; set; }
    public AppProvider? AppProvider { get; set; }
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RouteProvider>(entity =>
        {
            entity.ToTable("routify_route_providers");
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

            entity.Property(e => e.AppProviderId)
                .HasColumnName("app_provider_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Model)
                .HasColumnName("model")
                .HasMaxLength(256);
            
            entity.Property(e => e.Attrs)
                .HasColumnName("attrs")
                .HasColumnType("jsonb")
                .IsRequired()
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>(),
                    ValueComparers.StringDictionary);
            
            entity.Property(e => e.RetryConfig)
                .HasColumnName("retry_config")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<RetryConfig>(v));

            entity.Property(e => e.Weight)
                .HasColumnName("weight")
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");

            entity.Property(e => e.VersionId)
                .HasColumnName("version_id")
                .IsRequired()
                .HasMaxLength(30);
            
            entity.HasOne(e => e.Route)
                .WithMany(e => e.Providers)
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.App)
                .WithMany()
                .HasForeignKey(e => e.AppId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.AppProvider)
                .WithMany()
                .HasForeignKey(e => e.AppProviderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
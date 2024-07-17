using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Common;
using Routify.Data.Enums;
using Routify.Data.Utils;

namespace Routify.Data.Models;

public record Route
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    
    public RouteStrategy Strategy { get; set; }
    public RouteConfig Config { get; set; } = null!;
    public RateLimitConfig? RateLimitConfig { get; set; }
    public CacheConfig? CacheConfig { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    public string VersionId { get; set; } = null!;
    public RouteStatus Status { get; set; }
    
    public App? App { get; set; }
    public ICollection<RouteProvider> Providers { get; set; } = [];

    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Route>(entity =>
        {
            entity.ToTable("routify_routes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.AppId)
                .HasColumnName("app_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(512);

            entity.Property(e => e.Path)
                .HasColumnName("path")
                .IsRequired()
                .HasMaxLength(256);
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .IsRequired();
            
            entity.Property(e => e.Schema)
                .HasColumnName("schema")
                .IsRequired();
            
            entity.Property(e => e.Strategy)
                .HasColumnName("strategy")
                .IsRequired();

            entity.Property(e => e.Config)
                .HasColumnName("config")
                .HasColumnType("jsonb")
                .IsRequired()
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<RouteConfig>(v) ?? new RouteConfig());
            
            entity.Property(e => e.RateLimitConfig)
                .HasColumnName("rate_limit_config")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<RateLimitConfig>(v) ?? new RateLimitConfig());

            entity.Property(e => e.CacheConfig)
                .HasColumnName("cache_config")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<CacheConfig>(v) ?? new CacheConfig());
            
            entity.Property(e => e.Attrs)
                .HasColumnName("attrs")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>(),
                    ValueComparers.StringDictionary);
            
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

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();
            
            entity.HasOne(e => e.App)
                .WithMany()
                .HasForeignKey(e => e.AppId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(e => e.Providers)
                .WithOne(e => e.Route)
                .HasForeignKey(e => e.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
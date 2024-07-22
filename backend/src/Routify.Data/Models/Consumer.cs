using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Data.Models;

public record Consumer
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public string? Alias { get; set; }
    
    public CostLimitConfig? CostLimitConfig { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    public string VersionId { get; set; } = null!;
    public ConsumerStatus Status { get; set; }
    
    public App? App { get; set; }
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.ToTable("routify_consumers");
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
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasColumnName("description");
            
            entity.Property(e => e.Alias)
                .HasColumnName("alias")
                .HasMaxLength(100);
            
            entity.Property(e => e.CostLimitConfig)
                .HasColumnName("cost_limit_config")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<CostLimitConfig>(v) ?? new CostLimitConfig());
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");
            
            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(30);
            
            entity.Property(e => e.VersionId)
                .HasColumnName("version_id")
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();
        });
    }
}
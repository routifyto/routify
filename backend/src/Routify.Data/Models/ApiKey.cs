using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Data.Models;

public record ApiKey
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool CanUseGateway { get; set; }
    public AppRole? Role { get; set; }

    public string Prefix { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Suffix { get; set; } = null!;
    public ApiKeyHashAlgorithm Algorithm { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    public CostLimitConfig? CostLimitConfig { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    public string VersionId { get; set; } = null!;
    public ApiKeyStatus Status { get; set; }
    
    public App? App { get; set; }

    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.ToTable("routify_api_keys");
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
            
            entity.Property(e => e.CanUseGateway)
                .HasColumnName("can_use_gateway")
                .IsRequired();
            
            entity.Property(e => e.Role)
                .HasColumnName("role");
            
            entity.Property(e => e.Prefix)
                .HasColumnName("prefix")
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.Hash)
                .HasColumnName("hash")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Salt)
                .HasColumnName("salt")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Suffix)
                .HasColumnName("suffix")
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.Algorithm)
                .HasColumnName("algorithm")
                .IsRequired();
            
            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at");
            
            entity.Property(e => e.CostLimitConfig)
                .HasColumnName("cost_limit_config")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<CostLimitConfig>(v) ?? new CostLimitConfig());
            
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
        });
        
        modelBuilder.Entity<ApiKey>()
            .HasOne(e => e.App)
            .WithMany()
            .HasForeignKey(e => e.AppId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
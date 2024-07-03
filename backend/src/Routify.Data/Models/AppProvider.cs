using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Utils;

namespace Routify.Data.Models;

public record AppProvider
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Alias { get; set; } = null!;
    public string? Description { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string VersionId { get; set; } = null!;
    public AppProviderStatus Status { get; set; }
    
    public App? App { get; set; }

    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppProvider>(entity =>
        {
            entity.ToTable("routify_app_providers");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.AppId)
                .HasColumnName("app_id")
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(e => e.Provider)
                .HasColumnName("provider")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Alias)
                .HasColumnName("alias")
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(512);
            
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
        });
    }
}

public enum AppProviderStatus
{
    Active = 1,
    Inactive = 2
}
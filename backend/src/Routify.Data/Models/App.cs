using Microsoft.EntityFrameworkCore;

namespace Routify.Data.Models;

public record App
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string VersionId { get; set; } = null!;
    public AppStatus Status { get; set; } = AppStatus.Active;
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<App>(entity =>
        {
            entity.ToTable("routify_apps");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(512);

            entity.Property(e => e.Avatar)
                .HasColumnName("avatar")
                .HasMaxLength(256);
            
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
    }
}

public enum AppStatus
{
    Active = 1,
    Inactive = 2
}
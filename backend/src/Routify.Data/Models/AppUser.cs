using Microsoft.EntityFrameworkCore;

namespace Routify.Data.Models;

public record AppUser
{
    public string Id { get; set; } = null!;
    public string AppId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public AppUserRole Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string? UpdatedBy { get; set; }
    
    public App? App { get; set; }
    public User? User { get; set; }

    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("routify_app_users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.AppId)
                .HasColumnName("app_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Role)
                .HasColumnName("role")
                .IsRequired();

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
            
            entity.HasOne(e => e.App)
                .WithMany()
                .HasForeignKey(e => e.AppId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.AppId }).IsUnique();
            entity.HasIndex(e => e.AppId);
            entity.HasIndex(e => e.UserId);
        });
    }
}

public enum AppUserRole
{
    Owner = 1,
    Admin = 2,
    Member = 3
}
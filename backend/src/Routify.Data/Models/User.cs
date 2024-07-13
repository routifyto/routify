using Microsoft.EntityFrameworkCore;
using Routify.Core.Utils;
using Routify.Data.Enums;

namespace Routify.Data.Models;

public record User
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Password { get; set; }
    public UserAttrs? Attrs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserStatus Status { get; set; }
    
    internal static void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("routify_users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(30);

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Avatar)
                .HasColumnName("avatar")
                .HasMaxLength(256);
            
            entity.Property(e => e.Password)
                .HasColumnName("password")
                .HasMaxLength(256);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.Property(e => e.Attrs)
                .HasColumnName("attrs")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => RoutifyJsonSerializer.Serialize(v),
                    v => RoutifyJsonSerializer.Deserialize<UserAttrs>(v));

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}

public record UserAttrs
{
    public string? GoogleId { get; set; }
}
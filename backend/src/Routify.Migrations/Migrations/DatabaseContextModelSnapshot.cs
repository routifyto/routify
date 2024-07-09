﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Routify.Data;

#nullable disable

namespace Routify.Migrations.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Routify.Data.Models.App", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("avatar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("version_id");

                    b.HasKey("Id");

                    b.ToTable("routify_apps", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.AppProvider", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("alias");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<string>("Attrs")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("attrs");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("provider");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("version_id");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("routify_app_providers", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId", "AppId")
                        .IsUnique();

                    b.ToTable("routify_app_users", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.Route", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<string>("Attrs")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("attrs");

                    b.Property<string>("CacheConfig")
                        .HasColumnType("text")
                        .HasColumnName("cache_config");

                    b.Property<string>("Config")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("config");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("description");

                    b.Property<int>("InputType")
                        .HasColumnType("integer")
                        .HasColumnName("input_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("path");

                    b.Property<string>("RateLimitConfig")
                        .HasColumnType("text")
                        .HasColumnName("rate_limit_config");

                    b.Property<string>("RetryConfig")
                        .HasColumnType("text")
                        .HasColumnName("retry_config");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("version_id");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("routify_routes", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.RouteProvider", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<string>("AppProviderId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_provider_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Model")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("model");

                    b.Property<string>("RouteId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("route_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("version_id");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("AppProviderId");

                    b.HasIndex("RouteId");

                    b.ToTable("routify_route_providers", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.TextLog", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("ApiKeyId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("api_key_id");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<string>("AppProviderId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_provider_id");

                    b.Property<double>("Duration")
                        .HasColumnType("double precision")
                        .HasColumnName("duration");

                    b.Property<DateTime>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_at");

                    b.Property<double>("InputCost")
                        .HasColumnType("double precision")
                        .HasColumnName("input_cost");

                    b.Property<int>("InputTokens")
                        .HasColumnType("integer")
                        .HasColumnName("input_tokens");

                    b.Property<string>("Model")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("model");

                    b.Property<double>("OutputCost")
                        .HasColumnType("double precision")
                        .HasColumnName("output_cost");

                    b.Property<int>("OutputTokens")
                        .HasColumnType("integer")
                        .HasColumnName("output_tokens");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("path");

                    b.Property<string>("Provider")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("provider");

                    b.Property<string>("RequestBody")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("request_body");

                    b.Property<string>("ResponseBody")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("response_body");

                    b.Property<int>("ResponseStatusCode")
                        .HasColumnType("integer")
                        .HasColumnName("response_status_code");

                    b.Property<string>("RouteId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("route_id");

                    b.Property<string>("RouteProviderId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("route_provider_id");

                    b.Property<string>("SessionId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("session_id");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.HasKey("Id");

                    b.ToTable("routify_text_logs", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("Attrs")
                        .HasColumnType("jsonb")
                        .HasColumnName("attrs");

                    b.Property<string>("Avatar")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("avatar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("password");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("routify_users", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.AppProvider", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
                });

            modelBuilder.Entity("Routify.Data.Models.AppUser", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Routify.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Routify.Data.Models.Route", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
                });

            modelBuilder.Entity("Routify.Data.Models.RouteProvider", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Routify.Data.Models.AppProvider", "AppProvider")
                        .WithMany()
                        .HasForeignKey("AppProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Routify.Data.Models.Route", "Route")
                        .WithMany("Providers")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");

                    b.Navigation("AppProvider");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("Routify.Data.Models.Route", b =>
                {
                    b.Navigation("Providers");
                });
#pragma warning restore 612, 618
        }
    }
}

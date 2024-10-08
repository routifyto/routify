﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Routify.Data;

#nullable disable

namespace Routify.Migrations.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240724091819_InitDatabase")]
    partial class InitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Routify.Data.Models.ApiKey", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<int>("Algorithm")
                        .HasColumnType("integer")
                        .HasColumnName("algorithm");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<bool>("CanUseGateway")
                        .HasColumnType("boolean")
                        .HasColumnName("can_use_gateway");

                    b.Property<string>("CostLimitConfig")
                        .HasColumnType("jsonb")
                        .HasColumnName("cost_limit_config");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("hash");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("prefix");

                    b.Property<int?>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("salt");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("Suffix")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("suffix");

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

                    b.ToTable("routify_api_keys", (string)null);
                });

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

            modelBuilder.Entity("Routify.Data.Models.CompletionLog", b =>
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

                    b.Property<int>("CacheStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("cache_status");

                    b.Property<string>("ConsumerId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("consumer_id");

                    b.Property<double>("Duration")
                        .HasColumnType("double precision")
                        .HasColumnName("duration");

                    b.Property<DateTime>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_at");

                    b.Property<decimal>("InputCost")
                        .HasColumnType("numeric")
                        .HasColumnName("input_cost");

                    b.Property<int>("InputTokens")
                        .HasColumnType("integer")
                        .HasColumnName("input_tokens");

                    b.Property<string>("Model")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("model");

                    b.Property<int>("OutgoingRequestsCount")
                        .HasColumnType("integer")
                        .HasColumnName("outgoing_requests_count");

                    b.Property<decimal>("OutputCost")
                        .HasColumnType("numeric")
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
                        .HasColumnType("text")
                        .HasColumnName("request_body");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("jsonb")
                        .HasColumnName("request_headers");

                    b.Property<string>("RequestMethod")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("request_method");

                    b.Property<string>("RequestUrl")
                        .HasColumnType("text")
                        .HasColumnName("request_url");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("text")
                        .HasColumnName("response_body");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("jsonb")
                        .HasColumnName("response_headers");

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
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("session_id");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer")
                        .HasColumnName("status_code");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("routify_completion_logs", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.CompletionOutgoingLog", b =>
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

                    b.Property<double>("Duration")
                        .HasColumnType("double precision")
                        .HasColumnName("duration");

                    b.Property<DateTime>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_at");

                    b.Property<string>("IncomingLogId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("incoming_log_id");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("provider");

                    b.Property<string>("RequestBody")
                        .HasColumnType("text")
                        .HasColumnName("request_body");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("jsonb")
                        .HasColumnName("request_headers");

                    b.Property<string>("RequestMethod")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("request_method");

                    b.Property<string>("RequestUrl")
                        .HasColumnType("text")
                        .HasColumnName("request_url");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("text")
                        .HasColumnName("response_body");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("jsonb")
                        .HasColumnName("response_headers");

                    b.Property<int>("RetryCount")
                        .HasColumnType("integer")
                        .HasColumnName("retry_count");

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

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer")
                        .HasColumnName("status_code");

                    b.HasKey("Id");

                    b.ToTable("routify_completion_outgoing_logs", (string)null);
                });

            modelBuilder.Entity("Routify.Data.Models.Consumer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("id");

                    b.Property<string>("Alias")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("alias");

                    b.Property<string>("AppId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("app_id");

                    b.Property<string>("CostLimitConfig")
                        .HasColumnType("jsonb")
                        .HasColumnName("cost_limit_config");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("updated_by");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("version_id");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("routify_consumers", (string)null);
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
                        .HasColumnType("jsonb")
                        .HasColumnName("attrs");

                    b.Property<string>("CacheConfig")
                        .HasColumnType("jsonb")
                        .HasColumnName("cache_config");

                    b.Property<string>("CostLimitConfig")
                        .HasColumnType("jsonb")
                        .HasColumnName("cost_limit_config");

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

                    b.Property<bool>("IsFailoverEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_failover_enabled");

                    b.Property<bool>("IsLoadBalanceEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_load_balance_enabled");

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
                        .HasColumnType("jsonb")
                        .HasColumnName("rate_limit_config");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("schema");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int?>("Timeout")
                        .HasColumnType("integer")
                        .HasColumnName("timeout");

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

                    b.Property<int>("Index")
                        .HasColumnType("integer")
                        .HasColumnName("index");

                    b.Property<string>("Model")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("model");

                    b.Property<string>("RetryConfig")
                        .HasColumnType("jsonb")
                        .HasColumnName("retry_config");

                    b.Property<string>("RouteId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("route_id");

                    b.Property<int?>("Timeout")
                        .HasColumnType("integer")
                        .HasColumnName("timeout");

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

                    b.Property<int>("Weight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1)
                        .HasColumnName("weight");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("AppProviderId");

                    b.HasIndex("RouteId");

                    b.ToTable("routify_route_providers", (string)null);
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

            modelBuilder.Entity("Routify.Data.Models.ApiKey", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
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

            modelBuilder.Entity("Routify.Data.Models.Consumer", b =>
                {
                    b.HasOne("Routify.Data.Models.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
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

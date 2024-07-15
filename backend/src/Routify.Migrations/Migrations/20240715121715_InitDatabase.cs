using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routify_apps",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    avatar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_apps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "routify_completion_logs",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    route_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    app_provider_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    route_provider_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    api_key_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    consumer_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gateway_request = table.Column<string>(type: "jsonb", nullable: false),
                    provider_request = table.Column<string>(type: "jsonb", nullable: true),
                    gateway_response = table.Column<string>(type: "jsonb", nullable: true),
                    provider_response = table.Column<string>(type: "jsonb", nullable: true),
                    input_tokens = table.Column<int>(type: "integer", nullable: false),
                    output_tokens = table.Column<int>(type: "integer", nullable: false),
                    input_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    output_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_completion_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "routify_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    avatar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    attrs = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "routify_api_keys",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    can_use_gateway = table.Column<bool>(type: "boolean", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: true),
                    prefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    suffix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    algorithm = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_api_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_api_keys_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routify_app_providers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    attrs = table.Column<string>(type: "jsonb", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_app_providers", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_app_providers_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routify_consumers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    updated_by = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_consumers", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_consumers_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routify_routes",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    schema = table.Column<string>(type: "text", nullable: false),
                    config = table.Column<string>(type: "jsonb", nullable: false),
                    rate_limit_config = table.Column<string>(type: "jsonb", nullable: true),
                    cache_config = table.Column<string>(type: "jsonb", nullable: true),
                    attrs = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_routes", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_routes_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routify_app_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    user_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_app_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_app_users_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routify_app_users_routify_users_user_id",
                        column: x => x.user_id,
                        principalTable: "routify_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routify_route_providers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    route_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_provider_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    model = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    attrs = table.Column<string>(type: "jsonb", nullable: false),
                    retry_config = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    version_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_route_providers", x => x.id);
                    table.ForeignKey(
                        name: "FK_routify_route_providers_routify_app_providers_app_provider_~",
                        column: x => x.app_provider_id,
                        principalTable: "routify_app_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routify_route_providers_routify_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "routify_apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routify_route_providers_routify_routes_route_id",
                        column: x => x.route_id,
                        principalTable: "routify_routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_routify_api_keys_app_id",
                table: "routify_api_keys",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_app_providers_app_id",
                table: "routify_app_providers",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_app_users_app_id",
                table: "routify_app_users",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_app_users_user_id",
                table: "routify_app_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_app_users_user_id_app_id",
                table: "routify_app_users",
                columns: new[] { "user_id", "app_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routify_completion_logs_app_id",
                table: "routify_completion_logs",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_consumers_app_id",
                table: "routify_consumers",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_route_providers_app_id",
                table: "routify_route_providers",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_route_providers_app_provider_id",
                table: "routify_route_providers",
                column: "app_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_route_providers_route_id",
                table: "routify_route_providers",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_routes_app_id",
                table: "routify_routes",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_routify_users_email",
                table: "routify_users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routify_api_keys");

            migrationBuilder.DropTable(
                name: "routify_app_users");

            migrationBuilder.DropTable(
                name: "routify_completion_logs");

            migrationBuilder.DropTable(
                name: "routify_consumers");

            migrationBuilder.DropTable(
                name: "routify_route_providers");

            migrationBuilder.DropTable(
                name: "routify_users");

            migrationBuilder.DropTable(
                name: "routify_app_providers");

            migrationBuilder.DropTable(
                name: "routify_routes");

            migrationBuilder.DropTable(
                name: "routify_apps");
        }
    }
}

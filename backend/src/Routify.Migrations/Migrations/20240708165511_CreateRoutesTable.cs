using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoutesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    input_type = table.Column<int>(type: "integer", nullable: false),
                    config = table.Column<string>(type: "text", nullable: false),
                    retry_config = table.Column<string>(type: "text", nullable: true),
                    rate_limit_config = table.Column<string>(type: "text", nullable: true),
                    cache_config = table.Column<string>(type: "text", nullable: true),
                    attrs = table.Column<string>(type: "text", nullable: false),
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
                name: "routify_route_providers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    route_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    app_provider_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    model = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routify_route_providers");

            migrationBuilder.DropTable(
                name: "routify_routes");
        }
    }
}

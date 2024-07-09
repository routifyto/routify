using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CreateTextLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routify_text_logs",
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
                    session_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    request_body = table.Column<string>(type: "text", nullable: false),
                    response_status_code = table.Column<int>(type: "integer", nullable: false),
                    response_body = table.Column<string>(type: "text", nullable: false),
                    input_tokens = table.Column<int>(type: "integer", nullable: false),
                    output_tokens = table.Column<int>(type: "integer", nullable: false),
                    input_cost = table.Column<double>(type: "double precision", nullable: false),
                    output_cost = table.Column<double>(type: "double precision", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routify_text_logs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routify_text_logs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCostLimiAndCacheColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cost_limit_config",
                table: "routify_routes",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cache_status",
                table: "routify_completion_logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost_limit_config",
                table: "routify_routes");

            migrationBuilder.DropColumn(
                name: "cache_status",
                table: "routify_completion_logs");
        }
    }
}

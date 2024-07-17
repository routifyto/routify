using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRouteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "strategy",
                table: "routify_routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "weight",
                table: "routify_route_providers",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strategy",
                table: "routify_routes");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "routify_route_providers");
        }
    }
}

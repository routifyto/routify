using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routify.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCostLimitInApiKeysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cost_limit_config",
                table: "routify_api_keys",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost_limit_config",
                table: "routify_api_keys");
        }
    }
}

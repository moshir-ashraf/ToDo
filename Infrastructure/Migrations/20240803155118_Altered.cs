using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Altered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Clusters");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Clusters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Clusters");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Clusters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

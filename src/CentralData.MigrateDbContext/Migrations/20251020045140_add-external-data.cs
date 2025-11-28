using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralData.MigrateDbContext.Migrations
{
    /// <inheritdoc />
    public partial class addexternaldata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "Payload",
                table: "Products",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "Payload",
                table: "Categories",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "Pictures",
                table: "Categories",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payload",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Categories");
        }
    }
}

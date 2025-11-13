using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralData.MigrateDbContext.Migrations
{
    /// <inheritdoc />
    public partial class addbarenamecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BareName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BareName",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BareName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BareName",
                table: "Categories");
        }
    }
}

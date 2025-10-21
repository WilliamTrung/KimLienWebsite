using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralData.MigrateDbContext.Migrations
{
    /// <inheritdoc />
    public partial class updateviewcountfeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "ProductViews");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "ProductViewCredentials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "ProductViewCredentials");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "ProductViews",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

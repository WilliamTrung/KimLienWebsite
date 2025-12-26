using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralData.MigrateDbContext.Migrations
{
    /// <inheritdoc />
    public partial class pricingfeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMetadata_AspNetUsers_UserId",
                table: "UserMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMetadata",
                table: "UserMetadata");

            migrationBuilder.RenameTable(
                name: "UserMetadata",
                newName: "UserMetadatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMetadatas",
                table: "UserMetadatas",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "PricingRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FixedPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    MinPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingRecords_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PricingRecords_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PricingRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PricingRecords_CreatedBy",
                table: "PricingRecords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRecords_ModifiedBy",
                table: "PricingRecords",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRecords_ProductId",
                table: "PricingRecords",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMetadatas_AspNetUsers_UserId",
                table: "UserMetadatas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMetadatas_AspNetUsers_UserId",
                table: "UserMetadatas");

            migrationBuilder.DropTable(
                name: "PricingRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMetadatas",
                table: "UserMetadatas");

            migrationBuilder.RenameTable(
                name: "UserMetadatas",
                newName: "UserMetadata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMetadata",
                table: "UserMetadata",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMetadata_AspNetUsers_UserId",
                table: "UserMetadata",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

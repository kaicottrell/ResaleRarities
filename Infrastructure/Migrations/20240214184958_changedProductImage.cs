using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_RareImage_ImageId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ImageId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "RareImage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RareImage_ProductId",
                table: "RareImage",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RareImage_Product_ProductId",
                table: "RareImage",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RareImage_Product_ProductId",
                table: "RareImage");

            migrationBuilder.DropIndex(
                name: "IX_RareImage_ProductId",
                table: "RareImage");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "RareImage");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ImageId",
                table: "Product",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_RareImage_ImageId",
                table: "Product",
                column: "ImageId",
                principalTable: "RareImage",
                principalColumn: "Id");
        }
    }
}

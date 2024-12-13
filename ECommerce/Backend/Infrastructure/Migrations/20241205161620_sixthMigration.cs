using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sixthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salesMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<float>(type: "real", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryZipcode = table.Column<int>(type: "int", nullable: false),
                    DeliveryState = table.Column<int>(type: "int", nullable: false),
                    DeliveryCountry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salesMasters_Country_DeliveryCountry",
                        column: x => x.DeliveryCountry,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salesMasters_State_DeliveryState",
                        column: x => x.DeliveryState,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salesMasters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salesDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SaleQty = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salesDetails_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salesDetails_salesMasters_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "salesMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_salesDetails_InvoiceId",
                table: "salesDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_salesDetails_ProductId",
                table: "salesDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_salesMasters_DeliveryCountry",
                table: "salesMasters",
                column: "DeliveryCountry");

            migrationBuilder.CreateIndex(
                name: "IX_salesMasters_DeliveryState",
                table: "salesMasters",
                column: "DeliveryState");

            migrationBuilder.CreateIndex(
                name: "IX_salesMasters_UserId",
                table: "salesMasters",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salesDetails");

            migrationBuilder.DropTable(
                name: "salesMasters");
        }
    }
}

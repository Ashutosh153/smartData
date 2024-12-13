using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changesinstatre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StateDetails_Country_Country_Id",
                table: "StateDetails");

            migrationBuilder.DropIndex(
                name: "IX_StateDetails_Country_Id",
                table: "StateDetails");

            migrationBuilder.DropColumn(
                name: "Country_Id",
                table: "StateDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Country_Id",
                table: "StateDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StateDetails_Country_Id",
                table: "StateDetails",
                column: "Country_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StateDetails_Country_Country_Id",
                table: "StateDetails",
                column: "Country_Id",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

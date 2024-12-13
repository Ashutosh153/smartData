using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Specialisation_Specialisation_ID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserType_UserType_ID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserType",
                table: "UserType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialisation",
                table: "Specialisation");

            migrationBuilder.RenameTable(
                name: "UserType",
                newName: "UserTypes");

            migrationBuilder.RenameTable(
                name: "Specialisation",
                newName: "SpecialisationDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTypes",
                table: "UserTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialisationDetails",
                table: "SpecialisationDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SpecialisationDetails_Specialisation_ID",
                table: "Users",
                column: "Specialisation_ID",
                principalTable: "SpecialisationDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTypes_UserType_ID",
                table: "Users",
                column: "UserType_ID",
                principalTable: "UserTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SpecialisationDetails_Specialisation_ID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTypes_UserType_ID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTypes",
                table: "UserTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialisationDetails",
                table: "SpecialisationDetails");

            migrationBuilder.RenameTable(
                name: "UserTypes",
                newName: "UserType");

            migrationBuilder.RenameTable(
                name: "SpecialisationDetails",
                newName: "Specialisation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserType",
                table: "UserType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialisation",
                table: "Specialisation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Specialisation_Specialisation_ID",
                table: "Users",
                column: "Specialisation_ID",
                principalTable: "Specialisation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserType_UserType_ID",
                table: "Users",
                column: "UserType_ID",
                principalTable: "UserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

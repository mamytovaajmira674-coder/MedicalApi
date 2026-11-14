using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApi.Migrations
{
    /// <inheritdoc />
    public partial class RestrictSpravkaTypeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spravki_SpravkaTypes_SpravkaTypeId",
                table: "Spravki");

            migrationBuilder.AddForeignKey(
                name: "FK_Spravki_SpravkaTypes_SpravkaTypeId",
                table: "Spravki",
                column: "SpravkaTypeId",
                principalTable: "SpravkaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spravki_SpravkaTypes_SpravkaTypeId",
                table: "Spravki");

            migrationBuilder.AddForeignKey(
                name: "FK_Spravki_SpravkaTypes_SpravkaTypeId",
                table: "Spravki",
                column: "SpravkaTypeId",
                principalTable: "SpravkaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

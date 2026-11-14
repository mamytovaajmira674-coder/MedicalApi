using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalApi.Migrations
{
    /// <inheritdoc />
    public partial class RestrictPatientDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spravki_Patients_PatientId",
                table: "Spravki");

            migrationBuilder.AddForeignKey(
                name: "FK_Spravki_Patients_PatientId",
                table: "Spravki",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spravki_Patients_PatientId",
                table: "Spravki");

            migrationBuilder.AddForeignKey(
                name: "FK_Spravki_Patients_PatientId",
                table: "Spravki",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medication_DorsageForm_DosageFormId",
                table: "Medication");

            migrationBuilder.RenameColumn(
                name: "DosageFormId",
                table: "Medication",
                newName: "DorsageFormId");

            migrationBuilder.RenameIndex(
                name: "IX_Medication_DosageFormId",
                table: "Medication",
                newName: "IX_Medication_DorsageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication",
                column: "DorsageFormId",
                principalTable: "DorsageForm",
                principalColumn: "DorsageFormId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication");

            migrationBuilder.RenameColumn(
                name: "DorsageFormId",
                table: "Medication",
                newName: "DosageFormId");

            migrationBuilder.RenameIndex(
                name: "IX_Medication_DorsageFormId",
                table: "Medication",
                newName: "IX_Medication_DosageFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_DorsageForm_DosageFormId",
                table: "Medication",
                column: "DosageFormId",
                principalTable: "DorsageForm",
                principalColumn: "DorsageFormId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

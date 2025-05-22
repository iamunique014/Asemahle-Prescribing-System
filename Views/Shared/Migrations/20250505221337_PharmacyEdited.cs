using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class PharmacyEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Pharmacy",
                newName: "PhysicalAddress1");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalAddress2",
                table: "Pharmacy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_PharmacistId",
                table: "Pharmacy",
                column: "PharmacistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy",
                column: "PharmacistId",
                principalTable: "Pharmacist",
                principalColumn: "PharmacistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy");

            migrationBuilder.DropIndex(
                name: "IX_Pharmacy_PharmacistId",
                table: "Pharmacy");

            migrationBuilder.DropColumn(
                name: "PhysicalAddress2",
                table: "Pharmacy");

            migrationBuilder.RenameColumn(
                name: "PhysicalAddress1",
                table: "Pharmacy",
                newName: "Address");
        }
    }
}

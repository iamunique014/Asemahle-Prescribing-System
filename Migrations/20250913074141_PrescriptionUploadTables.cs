using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class PrescriptionUploadTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingRepeats",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "TotalRepeats",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "MedicationItems");

            migrationBuilder.AddColumn<int>(
                name: "RemainingRepeats",
                table: "MedicationItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRepeats",
                table: "MedicationItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingRepeats",
                table: "MedicationItems");

            migrationBuilder.DropColumn(
                name: "TotalRepeats",
                table: "MedicationItems");

            migrationBuilder.AddColumn<int>(
                name: "RemainingRepeats",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRepeats",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "MedicationItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixMedication2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Strength",
                table: "MedicationActiveIngredient",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Strength",
                table: "MedicationActiveIngredient");
        }
    }
}

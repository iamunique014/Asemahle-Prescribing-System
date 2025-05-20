using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Medication");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Strength",
                table: "Medication",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

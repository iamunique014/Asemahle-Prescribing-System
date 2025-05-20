using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class MedicationStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "MedicationStockOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "MedicationStockOrder");
        }
    }
}

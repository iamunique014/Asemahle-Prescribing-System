using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPrescriptionFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateIssued",
                table: "Prescriptions",
                newName: "PrescriptionDate");

            migrationBuilder.AddColumn<string>(
                name: "RawText",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawText",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "PrescriptionDate",
                table: "Prescriptions",
                newName: "DateIssued");
        }
    }
}

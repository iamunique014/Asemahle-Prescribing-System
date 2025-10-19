using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateConstraintCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PrescriptionOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionOrders_CustomerId",
                table: "PrescriptionOrders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionOrders_AspNetUsers_CustomerId",
                table: "PrescriptionOrders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionOrders_AspNetUsers_CustomerId",
                table: "PrescriptionOrders");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionOrders_CustomerId",
                table: "PrescriptionOrders");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PrescriptionOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

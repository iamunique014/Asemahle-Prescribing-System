using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockOrder",
                columns: table => new
                {
                    StockOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOrder", x => x.StockOrderId);
                    table.ForeignKey(
                        name: "FK_StockOrder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationStockOrder",
                columns: table => new
                {
                    MedicationStockOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOrderId = table.Column<int>(type: "int", nullable: false),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationStockOrder", x => x.MedicationStockOrderId);
                    table.ForeignKey(
                        name: "FK_MedicationStockOrder_Medication_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medication",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicationStockOrder_StockOrder_StockOrderId",
                        column: x => x.StockOrderId,
                        principalTable: "StockOrder",
                        principalColumn: "StockOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationStockOrder_MedicationId",
                table: "MedicationStockOrder",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationStockOrder_StockOrderId",
                table: "MedicationStockOrder",
                column: "StockOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOrder_SupplierId",
                table: "StockOrder",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationStockOrder");

            migrationBuilder.DropTable(
                name: "StockOrder");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrescriptionOrdersAddOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionOrders_Prescriptions_PrescriptionId",
                table: "PrescriptionOrders");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionOrders_PrescriptionId",
                table: "PrescriptionOrders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PrescriptionOrders");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "PrescriptionOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CollectedDate",
                table: "PrescriptionOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRepeatOrder",
                table: "PrescriptionOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyDate",
                table: "PrescriptionOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "PrescriptionOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MedicationItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityOrdered = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_MedicationItems_MedicationItemId",
                        column: x => x.MedicationItemId,
                        principalTable: "MedicationItems",
                        principalColumn: "MedicationItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_PrescriptionOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PrescriptionOrders",
                        principalColumn: "PrescriptionOrdersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MedicationItemId",
                table: "OrderItems",
                column: "MedicationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CollectedDate",
                table: "PrescriptionOrders");

            migrationBuilder.DropColumn(
                name: "IsRepeatOrder",
                table: "PrescriptionOrders");

            migrationBuilder.DropColumn(
                name: "ReadyDate",
                table: "PrescriptionOrders");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "PrescriptionOrders");

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "PrescriptionOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrescriptionId",
                table: "PrescriptionOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionOrders_PrescriptionId",
                table: "PrescriptionOrders",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionOrders_Prescriptions_PrescriptionId",
                table: "PrescriptionOrders",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class NewDataBase8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

           

            migrationBuilder.CreateTable(
                name: "MedicationActiveIngredient",
                columns: table => new
                {
                    MedicationActiveIngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    ActiveIngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationActiveIngredient", x => x.MedicationActiveIngredientId);
                    table.ForeignKey(
                        name: "FK_MedicationActiveIngredient_ActiveIngredients_ActiveIngredientId",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "ActiveIngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicationActiveIngredient_Medication_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medication",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Cascade);
                });

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CustomerAllergy");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "MedicationActiveIngredient");

            migrationBuilder.DropTable(
                name: "MedicationStockOrder");

            migrationBuilder.DropTable(
                name: "Pharmacy");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Medication");

            migrationBuilder.DropTable(
                name: "StockOrder");

            migrationBuilder.DropTable(
                name: "Pharmacist");

            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropTable(
                name: "DorsageForm");

            migrationBuilder.DropTable(
                name: "Supplier");
        }
    }
}

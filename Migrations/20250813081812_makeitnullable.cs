using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescribingSystem.Migrations
{
    /// <inheritdoc />
    public partial class makeitnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLogs_StockOrder_StockOrderId",
                table: "ApprovalLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedMedicationItems_ApprovedOrders_ApprovedOrderId",
                table: "ApprovedMedicationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAllergy_ActiveIngredients_ActiveIngredientId",
                table: "CustomerAllergy");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAllergy_Customer_CustomerId",
                table: "CustomerAllergy");

            migrationBuilder.DropForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication");

            migrationBuilder.DropForeignKey(
                name: "FK_Medication_Supplier_SupplierId",
                table: "Medication");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationActiveIngredient_ActiveIngredients_ActiveIngredientId",
                table: "MedicationActiveIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationActiveIngredient_Medication_MedicationId",
                table: "MedicationActiveIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationStockOrder_Medication_MedicationId",
                table: "MedicationStockOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationStockOrder_StockOrder_StockOrderId",
                table: "MedicationStockOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOrder_Supplier_SupplierId",
                table: "StockOrder");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "HealthCouncilRegistrationNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLogs_StockOrder_StockOrderId",
                table: "ApprovalLogs",
                column: "StockOrderId",
                principalTable: "StockOrder",
                principalColumn: "StockOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedMedicationItems_ApprovedOrders_ApprovedOrderId",
                table: "ApprovedMedicationItems",
                column: "ApprovedOrderId",
                principalTable: "ApprovedOrders",
                principalColumn: "ApprovedOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAllergy_ActiveIngredients_ActiveIngredientId",
                table: "CustomerAllergy",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAllergy_Customer_CustomerId",
                table: "CustomerAllergy",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication",
                column: "DorsageFormId",
                principalTable: "DorsageForm",
                principalColumn: "DorsageFormId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_Supplier_SupplierId",
                table: "Medication",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationActiveIngredient_ActiveIngredients_ActiveIngredientId",
                table: "MedicationActiveIngredient",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationActiveIngredient_Medication_MedicationId",
                table: "MedicationActiveIngredient",
                column: "MedicationId",
                principalTable: "Medication",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationStockOrder_Medication_MedicationId",
                table: "MedicationStockOrder",
                column: "MedicationId",
                principalTable: "Medication",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationStockOrder_StockOrder_StockOrderId",
                table: "MedicationStockOrder",
                column: "StockOrderId",
                principalTable: "StockOrder",
                principalColumn: "StockOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy",
                column: "PharmacistId",
                principalTable: "Pharmacist",
                principalColumn: "PharmacistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOrder_Supplier_SupplierId",
                table: "StockOrder",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLogs_StockOrder_StockOrderId",
                table: "ApprovalLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedMedicationItems_ApprovedOrders_ApprovedOrderId",
                table: "ApprovedMedicationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAllergy_ActiveIngredients_ActiveIngredientId",
                table: "CustomerAllergy");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAllergy_Customer_CustomerId",
                table: "CustomerAllergy");

            migrationBuilder.DropForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication");

            migrationBuilder.DropForeignKey(
                name: "FK_Medication_Supplier_SupplierId",
                table: "Medication");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationActiveIngredient_ActiveIngredients_ActiveIngredientId",
                table: "MedicationActiveIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationActiveIngredient_Medication_MedicationId",
                table: "MedicationActiveIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationStockOrder_Medication_MedicationId",
                table: "MedicationStockOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationStockOrder_StockOrder_StockOrderId",
                table: "MedicationStockOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOrder_Supplier_SupplierId",
                table: "StockOrder");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "HealthCouncilRegistrationNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLogs_StockOrder_StockOrderId",
                table: "ApprovalLogs",
                column: "StockOrderId",
                principalTable: "StockOrder",
                principalColumn: "StockOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedMedicationItems_ApprovedOrders_ApprovedOrderId",
                table: "ApprovedMedicationItems",
                column: "ApprovedOrderId",
                principalTable: "ApprovedOrders",
                principalColumn: "ApprovedOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAllergy_ActiveIngredients_ActiveIngredientId",
                table: "CustomerAllergy",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAllergy_Customer_CustomerId",
                table: "CustomerAllergy",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_DorsageForm_DorsageFormId",
                table: "Medication",
                column: "DorsageFormId",
                principalTable: "DorsageForm",
                principalColumn: "DorsageFormId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medication_Supplier_SupplierId",
                table: "Medication",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationActiveIngredient_ActiveIngredients_ActiveIngredientId",
                table: "MedicationActiveIngredient",
                column: "ActiveIngredientId",
                principalTable: "ActiveIngredients",
                principalColumn: "ActiveIngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationActiveIngredient_Medication_MedicationId",
                table: "MedicationActiveIngredient",
                column: "MedicationId",
                principalTable: "Medication",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationStockOrder_Medication_MedicationId",
                table: "MedicationStockOrder",
                column: "MedicationId",
                principalTable: "Medication",
                principalColumn: "MedicationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationStockOrder_StockOrder_StockOrderId",
                table: "MedicationStockOrder",
                column: "StockOrderId",
                principalTable: "StockOrder",
                principalColumn: "StockOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacy_Pharmacist_PharmacistId",
                table: "Pharmacy",
                column: "PharmacistId",
                principalTable: "Pharmacist",
                principalColumn: "PharmacistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOrder_Supplier_SupplierId",
                table: "StockOrder",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

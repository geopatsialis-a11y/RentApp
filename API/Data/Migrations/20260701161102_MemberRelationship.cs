using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class MemberRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "FileAttachments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeletedBy",
                table: "Payments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedBy",
                table: "Payments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInvites_CreatedBy",
                table: "MemberInvites",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInvites_DeletedBy",
                table: "MemberInvites",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInvites_UpdatedBy",
                table: "MemberInvites",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CreatedBy",
                table: "Invoices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DeletedBy",
                table: "Invoices",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UpdatedBy",
                table: "Invoices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_CreatedBy",
                table: "FileAttachments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_DeletedBy",
                table: "FileAttachments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_TenantId",
                table: "FileAttachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_UpdatedBy",
                table: "FileAttachments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedBy",
                table: "Customers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DeletedBy",
                table: "Customers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UpdatedBy",
                table: "Customers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CostAssetHists_CreatedBy",
                table: "CostAssetHists",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CostAssetHists_DeletedBy",
                table: "CostAssetHists",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CostAssetHists_TenantId",
                table: "CostAssetHists",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CostAssetHists_UpdatedBy",
                table: "CostAssetHists",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreatedBy",
                table: "Contracts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DeletedBy",
                table: "Contracts",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UpdatedBy",
                table: "Contracts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAssets_CreatedBy",
                table: "ContractAssets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAssets_DeletedBy",
                table: "ContractAssets",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAssets_TenantId",
                table: "ContractAssets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAssets_UpdatedBy",
                table: "ContractAssets",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CreatedBy",
                table: "Contacts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_DeletedBy",
                table: "Contacts",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_TenantId",
                table: "Contacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UpdatedBy",
                table: "Contacts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFields_CreatedBy",
                table: "AssetTypeFields",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFields_DeletedBy",
                table: "AssetTypeFields",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFields_UpdatedBy",
                table: "AssetTypeFields",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldOptions_CreatedBy",
                table: "AssetTypeFieldOptions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldOptions_DeletedBy",
                table: "AssetTypeFieldOptions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldOptions_TenantId",
                table: "AssetTypeFieldOptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldOptions_UpdatedBy",
                table: "AssetTypeFieldOptions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CreatedBy",
                table: "Assets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DeletedBy",
                table: "Assets",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UpdatedBy",
                table: "Assets",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAttributeValues_CreatedBy",
                table: "AssetAttributeValues",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAttributeValues_DeletedBy",
                table: "AssetAttributeValues",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAttributeValues_TenantId",
                table: "AssetAttributeValues",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAttributeValues_UpdatedBy",
                table: "AssetAttributeValues",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAttributeValues_Members_CreatedBy",
                table: "AssetAttributeValues",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAttributeValues_Members_DeletedBy",
                table: "AssetAttributeValues",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAttributeValues_Members_UpdatedBy",
                table: "AssetAttributeValues",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAttributeValues_Tenants_TenantId",
                table: "AssetAttributeValues",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Members_CreatedBy",
                table: "Assets",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Members_DeletedBy",
                table: "Assets",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Members_UpdatedBy",
                table: "Assets",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Tenants_TenantId",
                table: "Assets",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_CreatedBy",
                table: "AssetTypeFieldOptions",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_DeletedBy",
                table: "AssetTypeFieldOptions",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_UpdatedBy",
                table: "AssetTypeFieldOptions",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFieldOptions_Tenants_TenantId",
                table: "AssetTypeFieldOptions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFields_Members_CreatedBy",
                table: "AssetTypeFields",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFields_Members_DeletedBy",
                table: "AssetTypeFields",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFields_Members_UpdatedBy",
                table: "AssetTypeFields",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypeFields_Tenants_TenantId",
                table: "AssetTypeFields",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Members_CreatedBy",
                table: "Contacts",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Members_DeletedBy",
                table: "Contacts",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Members_UpdatedBy",
                table: "Contacts",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Tenants_TenantId",
                table: "Contacts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAssets_Members_CreatedBy",
                table: "ContractAssets",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAssets_Members_DeletedBy",
                table: "ContractAssets",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAssets_Members_UpdatedBy",
                table: "ContractAssets",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAssets_Tenants_TenantId",
                table: "ContractAssets",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_CreatedBy",
                table: "Contracts",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_DeletedBy",
                table: "Contracts",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_UpdatedBy",
                table: "Contracts",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Tenants_TenantId",
                table: "Contracts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CostAssetHists_Members_CreatedBy",
                table: "CostAssetHists",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CostAssetHists_Members_DeletedBy",
                table: "CostAssetHists",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostAssetHists_Members_UpdatedBy",
                table: "CostAssetHists",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostAssetHists_Tenants_TenantId",
                table: "CostAssetHists",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Members_CreatedBy",
                table: "Customers",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Members_DeletedBy",
                table: "Customers",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Members_UpdatedBy",
                table: "Customers",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Members_CreatedBy",
                table: "FileAttachments",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Members_DeletedBy",
                table: "FileAttachments",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Members_UpdatedBy",
                table: "FileAttachments",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Tenants_TenantId",
                table: "FileAttachments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Members_CreatedBy",
                table: "Invoices",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Members_DeletedBy",
                table: "Invoices",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Members_UpdatedBy",
                table: "Invoices",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInvites_Members_CreatedBy",
                table: "MemberInvites",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInvites_Members_DeletedBy",
                table: "MemberInvites",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInvites_Members_UpdatedBy",
                table: "MemberInvites",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Members_CreatedBy",
                table: "Payments",
                column: "CreatedBy",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Members_DeletedBy",
                table: "Payments",
                column: "DeletedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Members_UpdatedBy",
                table: "Payments",
                column: "UpdatedBy",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAttributeValues_Members_CreatedBy",
                table: "AssetAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAttributeValues_Members_DeletedBy",
                table: "AssetAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAttributeValues_Members_UpdatedBy",
                table: "AssetAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetAttributeValues_Tenants_TenantId",
                table: "AssetAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Members_CreatedBy",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Members_DeletedBy",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Members_UpdatedBy",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Tenants_TenantId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_CreatedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_DeletedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFieldOptions_Members_UpdatedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFieldOptions_Tenants_TenantId",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFields_Members_CreatedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFields_Members_DeletedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFields_Members_UpdatedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypeFields_Tenants_TenantId",
                table: "AssetTypeFields");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Members_CreatedBy",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Members_DeletedBy",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Members_UpdatedBy",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Tenants_TenantId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAssets_Members_CreatedBy",
                table: "ContractAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAssets_Members_DeletedBy",
                table: "ContractAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAssets_Members_UpdatedBy",
                table: "ContractAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractAssets_Tenants_TenantId",
                table: "ContractAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_CreatedBy",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_DeletedBy",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_UpdatedBy",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Tenants_TenantId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_CostAssetHists_Members_CreatedBy",
                table: "CostAssetHists");

            migrationBuilder.DropForeignKey(
                name: "FK_CostAssetHists_Members_DeletedBy",
                table: "CostAssetHists");

            migrationBuilder.DropForeignKey(
                name: "FK_CostAssetHists_Members_UpdatedBy",
                table: "CostAssetHists");

            migrationBuilder.DropForeignKey(
                name: "FK_CostAssetHists_Tenants_TenantId",
                table: "CostAssetHists");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Members_CreatedBy",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Members_DeletedBy",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Members_UpdatedBy",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Members_CreatedBy",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Members_DeletedBy",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Members_UpdatedBy",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Tenants_TenantId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Members_CreatedBy",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Members_DeletedBy",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Members_UpdatedBy",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInvites_Members_CreatedBy",
                table: "MemberInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInvites_Members_DeletedBy",
                table: "MemberInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInvites_Members_UpdatedBy",
                table: "MemberInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Members_CreatedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Members_DeletedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Members_UpdatedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DeletedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UpdatedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MemberInvites_CreatedBy",
                table: "MemberInvites");

            migrationBuilder.DropIndex(
                name: "IX_MemberInvites_DeletedBy",
                table: "MemberInvites");

            migrationBuilder.DropIndex(
                name: "IX_MemberInvites_UpdatedBy",
                table: "MemberInvites");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CreatedBy",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_DeletedBy",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_UpdatedBy",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_CreatedBy",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_DeletedBy",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_TenantId",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_UpdatedBy",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedBy",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DeletedBy",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UpdatedBy",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CostAssetHists_CreatedBy",
                table: "CostAssetHists");

            migrationBuilder.DropIndex(
                name: "IX_CostAssetHists_DeletedBy",
                table: "CostAssetHists");

            migrationBuilder.DropIndex(
                name: "IX_CostAssetHists_TenantId",
                table: "CostAssetHists");

            migrationBuilder.DropIndex(
                name: "IX_CostAssetHists_UpdatedBy",
                table: "CostAssetHists");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CreatedBy",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_DeletedBy",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_UpdatedBy",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_ContractAssets_CreatedBy",
                table: "ContractAssets");

            migrationBuilder.DropIndex(
                name: "IX_ContractAssets_DeletedBy",
                table: "ContractAssets");

            migrationBuilder.DropIndex(
                name: "IX_ContractAssets_TenantId",
                table: "ContractAssets");

            migrationBuilder.DropIndex(
                name: "IX_ContractAssets_UpdatedBy",
                table: "ContractAssets");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CreatedBy",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_DeletedBy",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_TenantId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_UpdatedBy",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFields_CreatedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFields_DeletedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFields_UpdatedBy",
                table: "AssetTypeFields");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFieldOptions_CreatedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFieldOptions_DeletedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFieldOptions_TenantId",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypeFieldOptions_UpdatedBy",
                table: "AssetTypeFieldOptions");

            migrationBuilder.DropIndex(
                name: "IX_Assets_CreatedBy",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_DeletedBy",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_UpdatedBy",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_AssetAttributeValues_CreatedBy",
                table: "AssetAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AssetAttributeValues_DeletedBy",
                table: "AssetAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AssetAttributeValues_TenantId",
                table: "AssetAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AssetAttributeValues_UpdatedBy",
                table: "AssetAttributeValues");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "FileAttachments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetTypeFieldOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "AssetTypeFields",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "AssetTypeFields",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxValue",
                table: "AssetTypeFields",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinValue",
                table: "AssetTypeFields",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "AssetTypeFields",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationRegex",
                table: "AssetTypeFields",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssetTypeFieldOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetTypeFieldId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypeFieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetTypeFieldOptions_AssetTypeFields_AssetTypeFieldId",
                        column: x => x.AssetTypeFieldId,
                        principalTable: "AssetTypeFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldOptions_AssetTypeFieldId",
                table: "AssetTypeFieldOptions",
                column: "AssetTypeFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetTypeFieldOptions");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "AssetTypeFields");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "AssetTypeFields");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "AssetTypeFields");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "AssetTypeFields");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "AssetTypeFields");

            migrationBuilder.DropColumn(
                name: "ValidationRegex",
                table: "AssetTypeFields");
        }
    }
}

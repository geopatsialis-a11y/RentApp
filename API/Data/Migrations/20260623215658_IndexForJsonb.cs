using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndexForJsonb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_asset_properties_jsonb",
                table: "Assets",
                column: "PropertiesJson")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "idx_asset_tenant_type",
                table: "Assets",
                columns: new[] { "TenantId", "AssetTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_asset_properties_jsonb",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "idx_asset_tenant_type",
                table: "Assets");
        }
    }
}

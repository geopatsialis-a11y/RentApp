using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RateUnitAndCostToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyLeaseCost",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "AcquisitionType",
                table: "Assets",
                newName: "RateUnit");

            migrationBuilder.RenameColumn(
                name: "AcquisitionCost",
                table: "Assets",
                newName: "Cost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RateUnit",
                table: "Assets",
                newName: "AcquisitionType");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Assets",
                newName: "AcquisitionCost");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyLeaseCost",
                table: "Assets",
                type: "numeric(18,2)",
                nullable: true);
        }
    }
}

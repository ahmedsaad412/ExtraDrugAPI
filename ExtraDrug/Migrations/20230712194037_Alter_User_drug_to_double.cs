using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class Alter_User_drug_to_double : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coordsLongitude",
                table: "UsersDrugs",
                newName: "CoordsLongitude");

            migrationBuilder.RenameColumn(
                name: "coordsLatitude",
                table: "UsersDrugs",
                newName: "CoordsLatitude");

            migrationBuilder.AlterColumn<double>(
                name: "CoordsLongitude",
                table: "UsersDrugs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "CoordsLatitude",
                table: "UsersDrugs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoordsLongitude",
                table: "UsersDrugs",
                newName: "coordsLongitude");

            migrationBuilder.RenameColumn(
                name: "CoordsLatitude",
                table: "UsersDrugs",
                newName: "coordsLatitude");

            migrationBuilder.AlterColumn<decimal>(
                name: "coordsLongitude",
                table: "UsersDrugs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "coordsLatitude",
                table: "UsersDrugs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}

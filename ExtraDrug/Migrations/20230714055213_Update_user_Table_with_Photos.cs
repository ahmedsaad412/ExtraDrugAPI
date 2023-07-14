using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class Update_user_Table_with_Photos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhotoAPIPath",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoSysPath",
                table: "AspNetUsers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoAPIPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoSysPath",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class Add_photo_Table_ToUserDrug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDrugsPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APIPath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SysPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    UserDrugId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDrugsPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDrugsPhotos_UsersDrugs_UserDrugId",
                        column: x => x.UserDrugId,
                        principalTable: "UsersDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDrugsPhotos_UserDrugId",
                table: "UserDrugsPhotos",
                column: "UserDrugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDrugsPhotos");
        }
    }
}

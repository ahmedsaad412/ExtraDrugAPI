using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EffectiveMatrials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffectiveMatrials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ar_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    En_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Parcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsTradingPermitted = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drugs_DrugCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DrugCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drugs_DrugCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DrugCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drugs_DrugTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DrugTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugEffectiveMatrials",
                columns: table => new
                {
                    EffectiveMatrialsId = table.Column<int>(type: "int", nullable: false),
                    InDrugsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugEffectiveMatrials", x => new { x.EffectiveMatrialsId, x.InDrugsId });
                    table.ForeignKey(
                        name: "FK_DrugEffectiveMatrials_Drugs_InDrugsId",
                        column: x => x.InDrugsId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugEffectiveMatrials_EffectiveMatrials_EffectiveMatrialsId",
                        column: x => x.EffectiveMatrialsId,
                        principalTable: "EffectiveMatrials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugEffectiveMatrials_InDrugsId",
                table: "DrugEffectiveMatrials",
                column: "InDrugsId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_CategoryId",
                table: "Drugs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_CompanyId",
                table: "Drugs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_TypeId",
                table: "Drugs",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugEffectiveMatrials");

            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropTable(
                name: "EffectiveMatrials");

            migrationBuilder.DropTable(
                name: "DrugCategories");

            migrationBuilder.DropTable(
                name: "DrugCompanies");

            migrationBuilder.DropTable(
                name: "DrugTypes");
        }
    }
}

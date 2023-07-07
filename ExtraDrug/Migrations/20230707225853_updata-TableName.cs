using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class updataTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugEffectiveMatrials");

            migrationBuilder.CreateTable(
                name: "DrugEffectiveMatrial",
                columns: table => new
                {
                    EffectiveMatrialsId = table.Column<int>(type: "int", nullable: false),
                    InDrugsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugEffectiveMatrial", x => new { x.EffectiveMatrialsId, x.InDrugsId });
                    table.ForeignKey(
                        name: "FK_DrugEffectiveMatrial_Drugs_InDrugsId",
                        column: x => x.InDrugsId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugEffectiveMatrial_EffectiveMatrials_EffectiveMatrialsId",
                        column: x => x.EffectiveMatrialsId,
                        principalTable: "EffectiveMatrials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugEffectiveMatrial_InDrugsId",
                table: "DrugEffectiveMatrial",
                column: "InDrugsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugEffectiveMatrial");

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
        }
    }
}

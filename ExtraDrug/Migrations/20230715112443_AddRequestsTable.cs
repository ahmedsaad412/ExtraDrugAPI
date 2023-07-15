using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UsersDrugs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DrugRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugRequests_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrugRequests_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestItem",
                columns: table => new
                {
                    UserDrugId = table.Column<int>(type: "int", nullable: false),
                    DrugRequestId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItem", x => new { x.UserDrugId, x.DrugRequestId });
                    table.ForeignKey(
                        name: "FK_RequestItem_DrugRequests_DrugRequestId",
                        column: x => x.DrugRequestId,
                        principalTable: "DrugRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestItem_UsersDrugs_UserDrugId",
                        column: x => x.UserDrugId,
                        principalTable: "UsersDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugRequests_DonorId",
                table: "DrugRequests",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugRequests_ReceiverId",
                table: "DrugRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItem_DrugRequestId",
                table: "RequestItem",
                column: "DrugRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestItem");

            migrationBuilder.DropTable(
                name: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UsersDrugs");
        }
    }
}

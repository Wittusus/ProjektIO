using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIO.Migrations
{
    public partial class CreateHedgefundsHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HedgefundsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReturnRate = table.Column<double>(type: "double", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HedgefundId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HedgefundsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HedgefundsHistory_Hedgefunds_HedgefundId",
                        column: x => x.HedgefundId,
                        principalTable: "Hedgefunds",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HedgefundsHistory_HedgefundId",
                table: "HedgefundsHistory",
                column: "HedgefundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HedgefundsHistory");
        }
    }
}

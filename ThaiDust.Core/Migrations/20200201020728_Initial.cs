using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThaiDust.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(nullable: false),
                    StationCode = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => new { x.DateTime, x.StationCode });
                    table.ForeignKey(
                        name: "FK_Records_Stations_StationCode",
                        column: x => x.StationCode,
                        principalTable: "Stations",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_StationCode",
                table: "Records",
                column: "StationCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}

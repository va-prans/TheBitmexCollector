using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBitmexCollector.Migrations
{
    public partial class TradeBins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeBins",
                columns: table => new
                {
                    TradeBinId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Symbol = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Open = table.Column<decimal>(nullable: false),
                    High = table.Column<decimal>(nullable: false),
                    Low = table.Column<decimal>(nullable: false),
                    Close = table.Column<decimal>(nullable: false),
                    HomeNotional = table.Column<decimal>(nullable: false),
                    ForeignNotional = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: true),
                    Trades = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeBins", x => x.TradeBinId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeBins_Symbol",
                table: "TradeBins",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_TradeBins_Timestamp",
                table: "TradeBins",
                column: "Timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeBins");
        }
    }
}

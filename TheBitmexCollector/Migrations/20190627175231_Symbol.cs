using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBitmexCollector.Migrations
{
    public partial class Symbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Liquidations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Liquidations_Symbol",
                table: "Liquidations",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Liquidations_Symbol",
                table: "Liquidations");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Liquidations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBitmexCollector.Migrations
{
    public partial class TradeBins1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "TradeBins",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "TradeBins",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}

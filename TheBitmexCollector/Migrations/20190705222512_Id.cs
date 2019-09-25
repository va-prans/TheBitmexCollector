using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBitmexCollector.Migrations
{
    public partial class Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LiquidationId",
                table: "Liquidations",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySQL:AutoIncrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LiquidationId",
                table: "Liquidations",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("MySQL:AutoIncrement", true);
        }
    }
}

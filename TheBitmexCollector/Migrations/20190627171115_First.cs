using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBitmexCollector.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Liquidations",
                columns: table => new
                {
                    LiquidationId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Direction = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liquidations", x => x.LiquidationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Liquidations");
        }
    }
}

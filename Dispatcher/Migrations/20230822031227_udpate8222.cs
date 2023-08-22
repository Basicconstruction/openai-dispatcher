using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Migrations
{
    public partial class udpate8222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "decimal(16,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "OpenKeys",
                type: "decimal(16,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,8)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "decimal(8,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "OpenKeys",
                type: "decimal(8,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,8)",
                oldNullable: true);
        }
    }
}

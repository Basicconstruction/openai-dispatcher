using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Migrations
{
    public partial class update822 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "decimal(8,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "PoolKeys",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "OpenKeys",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "PoolKeys");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "OpenKeys");
        }
    }
}

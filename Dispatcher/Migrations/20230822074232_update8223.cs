using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Migrations
{
    public partial class update8223 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HandHosts",
                table: "PoolKeys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HandHosts",
                table: "PoolKeys");
        }
    }
}

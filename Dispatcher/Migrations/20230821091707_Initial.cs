using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoolKeys",
                columns: table => new
                {
                    PoolKeyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cipher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hosts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolKeys", x => x.PoolKeyId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    KeyUserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.KeyUserId);
                });

            migrationBuilder.CreateTable(
                name: "OpenKeys",
                columns: table => new
                {
                    OpenKeyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricingMethod = table.Column<int>(type: "int", nullable: false),
                    AvailableRequest = table.Column<int>(type: "int", nullable: false),
                    AvailableRequestToken = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(8,8)", nullable: false),
                    KeyUserId = table.Column<int>(type: "int", nullable: false),
                    KeyUserId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenKeys", x => x.OpenKeyId);
                    table.ForeignKey(
                        name: "FK_OpenKeys_Users_KeyUserId1",
                        column: x => x.KeyUserId1,
                        principalTable: "Users",
                        principalColumn: "KeyUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenKeys_KeyUserId1",
                table: "OpenKeys",
                column: "KeyUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenKeys");

            migrationBuilder.DropTable(
                name: "PoolKeys");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

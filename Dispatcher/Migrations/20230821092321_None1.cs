using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Migrations
{
    public partial class None1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenKeys_Users_KeyUserId1",
                table: "OpenKeys");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "PoolKeys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PricingMethod",
                table: "OpenKeys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "KeyUserId1",
                table: "OpenKeys",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "KeyUserId",
                table: "OpenKeys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "OpenKeys",
                type: "decimal(8,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,8)");

            migrationBuilder.AlterColumn<long>(
                name: "AvailableRequestToken",
                table: "OpenKeys",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableRequest",
                table: "OpenKeys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenKeys_Users_KeyUserId1",
                table: "OpenKeys",
                column: "KeyUserId1",
                principalTable: "Users",
                principalColumn: "KeyUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenKeys_Users_KeyUserId1",
                table: "OpenKeys");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "PoolKeys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PricingMethod",
                table: "OpenKeys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "KeyUserId1",
                table: "OpenKeys",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KeyUserId",
                table: "OpenKeys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "OpenKeys",
                type: "decimal(8,8)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AvailableRequestToken",
                table: "OpenKeys",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailableRequest",
                table: "OpenKeys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenKeys_Users_KeyUserId1",
                table: "OpenKeys",
                column: "KeyUserId1",
                principalTable: "Users",
                principalColumn: "KeyUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

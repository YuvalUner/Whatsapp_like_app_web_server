using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class thanksEntityFramework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "test",
                table: "RefreshToken");

            migrationBuilder.UpdateData(
                table: "RefreshToken",
                keyColumn: "RegisteredUserusername",
                keyValue: null,
                column: "RegisteredUserusername",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RegisteredUserusername",
                table: "RefreshToken",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
                table: "RefreshToken",
                column: "RegisteredUserusername",
                principalTable: "RegisteredUser",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
                table: "RefreshToken");

            migrationBuilder.AlterColumn<string>(
                name: "RegisteredUserusername",
                table: "RefreshToken",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "RefreshToken",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "test",
                table: "RefreshToken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
                table: "RefreshToken",
                column: "RegisteredUserusername",
                principalTable: "RegisteredUser",
                principalColumn: "username");
        }
    }
}

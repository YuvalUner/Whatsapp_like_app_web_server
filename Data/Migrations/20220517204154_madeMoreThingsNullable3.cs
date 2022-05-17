using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class madeMoreThingsNullable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Message",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Message",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
                table: "RefreshToken",
                column: "RegisteredUserusername",
                principalTable: "RegisteredUser",
                principalColumn: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_RegisteredUser_RegisteredUserusername",
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

            migrationBuilder.UpdateData(
                table: "Message",
                keyColumn: "type",
                keyValue: null,
                column: "type",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Message",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Message",
                keyColumn: "content",
                keyValue: null,
                column: "content",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Message",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
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
    }
}

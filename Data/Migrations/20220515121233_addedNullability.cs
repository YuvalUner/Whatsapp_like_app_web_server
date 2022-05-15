using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addedNullability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "verificationcode",
                table: "PendingUser",
                newName: "verificationCode");

            migrationBuilder.AlterColumn<string>(
                name: "verificationcode",
                table: "RegisteredUser",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "verificationCode",
                table: "PendingUser",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "verificationCode",
                table: "PendingUser",
                newName: "verificationcode");

            migrationBuilder.UpdateData(
                table: "RegisteredUser",
                keyColumn: "verificationcode",
                keyValue: null,
                column: "verificationcode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "verificationcode",
                table: "RegisteredUser",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PendingUser",
                keyColumn: "verificationcode",
                keyValue: null,
                column: "verificationcode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "verificationcode",
                table: "PendingUser",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}

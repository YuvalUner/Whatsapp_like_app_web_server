using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class updatedMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Message",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "sender",
                table: "Message",
                newName: "sent");

            migrationBuilder.RenameColumn(
                name: "key",
                table: "Message",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "last",
                table: "Contact",
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
                name: "sent",
                table: "Message",
                newName: "sender");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Message",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Message",
                newName: "key");

            migrationBuilder.UpdateData(
                table: "Contact",
                keyColumn: "last",
                keyValue: null,
                column: "last",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "last",
                table: "Contact",
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

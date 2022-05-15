using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addedUsernameToRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegisteredUserusername",
                table: "RefreshToken",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "RefreshToken",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RegisteredUserusername",
                table: "RefreshToken",
                column: "RegisteredUserusername");

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

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_RegisteredUserusername",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "RegisteredUserusername",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "RefreshToken");
        }
    }
}

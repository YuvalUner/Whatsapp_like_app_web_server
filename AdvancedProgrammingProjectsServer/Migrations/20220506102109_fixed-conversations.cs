using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProgrammingProjectsServer.Migrations
{
    public partial class fixedconversations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegisteredUserusername",
                table: "Conversation",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_RegisteredUserusername",
                table: "Conversation",
                column: "RegisteredUserusername");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversation_RegisteredUser_RegisteredUserusername",
                table: "Conversation",
                column: "RegisteredUserusername",
                principalTable: "RegisteredUser",
                principalColumn: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversation_RegisteredUser_RegisteredUserusername",
                table: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_Conversation_RegisteredUserusername",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "RegisteredUserusername",
                table: "Conversation");
        }
    }
}

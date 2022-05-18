using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class changedNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredUser_SecretQuestion_secretQuestionsId",
                table: "RegisteredUser");

            migrationBuilder.RenameColumn(
                name: "secretQuestionsId",
                table: "RegisteredUser",
                newName: "secretQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_RegisteredUser_secretQuestionsId",
                table: "RegisteredUser",
                newName: "IX_RegisteredUser_secretQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredUser_SecretQuestion_secretQuestionId",
                table: "RegisteredUser",
                column: "secretQuestionId",
                principalTable: "SecretQuestion",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredUser_SecretQuestion_secretQuestionId",
                table: "RegisteredUser");

            migrationBuilder.RenameColumn(
                name: "secretQuestionId",
                table: "RegisteredUser",
                newName: "secretQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_RegisteredUser_secretQuestionId",
                table: "RegisteredUser",
                newName: "IX_RegisteredUser_secretQuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredUser_SecretQuestion_secretQuestionsId",
                table: "RegisteredUser",
                column: "secretQuestionsId",
                principalTable: "SecretQuestion",
                principalColumn: "Id");
        }
    }
}

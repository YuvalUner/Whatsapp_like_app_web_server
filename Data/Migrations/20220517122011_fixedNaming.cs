using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class fixedNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingUser_SecretQuestion_secretQuestionsId",
                table: "PendingUser");

            migrationBuilder.RenameColumn(
                name: "secretQuestionsId",
                table: "PendingUser",
                newName: "secretQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingUser_secretQuestionsId",
                table: "PendingUser",
                newName: "IX_PendingUser_secretQuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "PendingUser",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "PendingUser",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "hashingAlgorithm",
                table: "PendingUser",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingUser_SecretQuestion_secretQuestionId",
                table: "PendingUser",
                column: "secretQuestionId",
                principalTable: "SecretQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingUser_SecretQuestion_secretQuestionId",
                table: "PendingUser");

            migrationBuilder.RenameColumn(
                name: "secretQuestionId",
                table: "PendingUser",
                newName: "secretQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_PendingUser_secretQuestionId",
                table: "PendingUser",
                newName: "IX_PendingUser_secretQuestionsId");

            migrationBuilder.UpdateData(
                table: "PendingUser",
                keyColumn: "salt",
                keyValue: null,
                column: "salt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "PendingUser",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PendingUser",
                keyColumn: "phone",
                keyValue: null,
                column: "phone",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "PendingUser",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "PendingUser",
                keyColumn: "hashingAlgorithm",
                keyValue: null,
                column: "hashingAlgorithm",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "hashingAlgorithm",
                table: "PendingUser",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingUser_SecretQuestion_secretQuestionsId",
                table: "PendingUser",
                column: "secretQuestionsId",
                principalTable: "SecretQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

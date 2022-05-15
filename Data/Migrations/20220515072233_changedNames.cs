using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class changedNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "encryptionAlgorithm",
                table: "RegisteredUser",
                newName: "hashingAlgorithm");

            migrationBuilder.RenameColumn(
                name: "encryptionAlgorithm",
                table: "PendingUser",
                newName: "hashingAlgorithm");

            migrationBuilder.AddColumn<DateTime>(
                name: "verificationCodeCreationTime",
                table: "PendingUser",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verificationCodeCreationTime",
                table: "PendingUser");

            migrationBuilder.RenameColumn(
                name: "hashingAlgorithm",
                table: "RegisteredUser",
                newName: "encryptionAlgorithm");

            migrationBuilder.RenameColumn(
                name: "hashingAlgorithm",
                table: "PendingUser",
                newName: "encryptionAlgorithm");
        }
    }
}

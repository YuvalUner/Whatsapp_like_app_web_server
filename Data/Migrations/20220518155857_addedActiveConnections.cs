using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addedActiveConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveConnection",
                columns: table => new
                {
                    Username = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConnectionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveConnection", x => new { x.ConnectionId, x.UserAgent, x.Username });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveConnection");
        }
    }
}

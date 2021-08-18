using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.EntityFrameworkCore.Migrations
{
    public partial class t2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureKey",
                table: "GuroupData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureKey",
                table: "GuroupData");
        }
    }
}

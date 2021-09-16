using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.EntityFrameworkCore.Migrations
{
    public partial class t2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SendId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Receiving = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeadPortrait = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marking = table.Column<sbyte>(type: "tinyint", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GuroupData_Id",
                table: "GuroupData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_Id",
                table: "GroupMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_Id",
                table: "Friends",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CreateFriends_Id",
                table: "CreateFriends",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_Id",
                table: "ChatMessage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_Receiving",
                table: "ChatMessage",
                column: "Receiving");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_User_Id",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_GuroupData_Id",
                table: "GuroupData");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_Id",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_Friends_Id",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_CreateFriends_Id",
                table: "CreateFriends");
        }
    }
}

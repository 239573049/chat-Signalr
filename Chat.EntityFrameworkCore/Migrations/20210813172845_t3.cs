using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.EntityFrameworkCore.Migrations
{
    public partial class t3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_SelfId",
                table: "GroupMembers",
                column: "SelfId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_User_SelfId",
                table: "GroupMembers",
                column: "SelfId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_User_SelfId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_SelfId",
                table: "GroupMembers");
        }
    }
}

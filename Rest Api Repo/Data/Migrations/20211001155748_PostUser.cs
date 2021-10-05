using Microsoft.EntityFrameworkCore.Migrations;

namespace Rest_Api_Repo.Data.Migrations
{
    public partial class PostUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "post",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                schema: "post",
                table: "Post",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_UserId",
                schema: "post",
                table: "Post",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_UserId",
                schema: "post",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserId",
                schema: "post",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "post",
                table: "Post");
        }
    }
}

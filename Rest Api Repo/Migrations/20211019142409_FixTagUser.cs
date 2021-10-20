using Microsoft.EntityFrameworkCore.Migrations;

namespace Rest_Api_Repo.Migrations
{
    public partial class FixTagUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_AspNetUsers_UserId",
                schema: "post",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_UserId",
                schema: "post",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "post",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "UserCreatorId",
                schema: "post",
                table: "Tag",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserCreatorId",
                schema: "post",
                table: "Tag",
                column: "UserCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_AspNetUsers_UserCreatorId",
                schema: "post",
                table: "Tag",
                column: "UserCreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_AspNetUsers_UserCreatorId",
                schema: "post",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_UserCreatorId",
                schema: "post",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UserCreatorId",
                schema: "post",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "post",
                table: "Tag",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                schema: "post",
                table: "Tag",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_AspNetUsers_UserId",
                schema: "post",
                table: "Tag",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

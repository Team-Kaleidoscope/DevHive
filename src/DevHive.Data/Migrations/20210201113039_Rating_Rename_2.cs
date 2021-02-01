using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
    public partial class Rating_Rename_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatedPost_AspNetUsers_UserId",
                table: "RatedPost");

            migrationBuilder.DropForeignKey(
                name: "FK_RatedPost_Posts_PostId",
                table: "RatedPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedPost",
                table: "RatedPost");

            migrationBuilder.RenameTable(
                name: "RatedPost",
                newName: "RatedPosts");

            migrationBuilder.RenameIndex(
                name: "IX_RatedPost_PostId",
                table: "RatedPosts",
                newName: "IX_RatedPosts_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedPosts",
                table: "RatedPosts",
                columns: new[] { "UserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RatedPosts_AspNetUsers_UserId",
                table: "RatedPosts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatedPosts_Posts_PostId",
                table: "RatedPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatedPosts_AspNetUsers_UserId",
                table: "RatedPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_RatedPosts_Posts_PostId",
                table: "RatedPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedPosts",
                table: "RatedPosts");

            migrationBuilder.RenameTable(
                name: "RatedPosts",
                newName: "RatedPost");

            migrationBuilder.RenameIndex(
                name: "IX_RatedPosts_PostId",
                table: "RatedPost",
                newName: "IX_RatedPost_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedPost",
                table: "RatedPost",
                columns: new[] { "UserId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RatedPost_AspNetUsers_UserId",
                table: "RatedPost",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatedPost_Posts_PostId",
                table: "RatedPost",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

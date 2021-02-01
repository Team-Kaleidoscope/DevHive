using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
    public partial class Rating_Frozen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rating_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Rating_RatingId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_RatingId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Rating",
                newName: "Rate");

            migrationBuilder.CreateTable(
                name: "UserRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Rate = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_UserId",
                table: "UserRates",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRates");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Rating",
                newName: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Rating",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Rating",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "Posts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RatingId",
                table: "Posts",
                column: "RatingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RatingId",
                table: "AspNetUsers",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rating_RatingId",
                table: "AspNetUsers",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Rating_RatingId",
                table: "Posts",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

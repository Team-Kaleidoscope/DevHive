using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
    public partial class Rating_ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rating_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "AspNetUsers");
        }
    }
}

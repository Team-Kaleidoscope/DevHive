using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
    public partial class User_Implements_Languages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "IssuerId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "TimeCreated");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Technologies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Languages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_UserId",
                table: "Technologies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UserId",
                table: "Languages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_AspNetUsers_UserId",
                table: "Languages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_AspNetUsers_UserId",
                table: "Technologies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_AspNetUsers_UserId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_AspNetUsers_UserId",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "IX_Technologies_UserId",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "IX_Languages_UserId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Languages");

            migrationBuilder.RenameColumn(
                name: "TimeCreated",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "IssuerId",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "ProfilePicture");
        }
    }
}

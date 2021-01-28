using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
    public partial class RelationsManuallyConfiguredTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_AspNetUsers_FriendsId",
                table: "UserUser");

            migrationBuilder.RenameColumn(
                name: "FriendsId",
                table: "UserUser",
                newName: "FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_FriendsId",
                table: "UserUser",
                newName: "IX_UserUser_FriendId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserUser",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser",
                columns: new[] { "UserId", "FriendId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_AspNetUsers_FriendId",
                table: "UserUser",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_AspNetUsers_UserId",
                table: "UserUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_AspNetUsers_FriendId",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_AspNetUsers_UserId",
                table: "UserUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "UserUser",
                newName: "FriendsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_FriendId",
                table: "UserUser",
                newName: "IX_UserUser_FriendsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_AspNetUsers_FriendsId",
                table: "UserUser",
                column: "FriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

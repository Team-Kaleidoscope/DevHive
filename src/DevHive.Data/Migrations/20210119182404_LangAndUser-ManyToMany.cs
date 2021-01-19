using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevHive.Data.Migrations
{
	public partial class LangAndUserManyToMany : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
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

			migrationBuilder.AddColumn<Guid>(
				name: "PostId",
				table: "Comments",
				type: "uuid",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "LanguageUser",
				columns: table => new
				{
					LanguagesId = table.Column<Guid>(type: "uuid", nullable: false),
					UsersId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_LanguageUser", x => new { x.LanguagesId, x.UsersId });
					table.ForeignKey(
						name: "FK_LanguageUser_AspNetUsers_UsersId",
						column: x => x.UsersId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_LanguageUser_Languages_LanguagesId",
						column: x => x.LanguagesId,
						principalTable: "Languages",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Posts",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					IssuerId = table.Column<Guid>(type: "uuid", nullable: false),
					TimeCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					Message = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Posts", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "TechnologyUser",
				columns: table => new
				{
					TechnologiesId = table.Column<Guid>(type: "uuid", nullable: false),
					UsersId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TechnologyUser", x => new { x.TechnologiesId, x.UsersId });
					table.ForeignKey(
						name: "FK_TechnologyUser_AspNetUsers_UsersId",
						column: x => x.UsersId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_TechnologyUser_Technologies_TechnologiesId",
						column: x => x.TechnologiesId,
						principalTable: "Technologies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Comments_PostId",
				table: "Comments",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_LanguageUser_UsersId",
				table: "LanguageUser",
				column: "UsersId");

			migrationBuilder.CreateIndex(
				name: "IX_TechnologyUser_UsersId",
				table: "TechnologyUser",
				column: "UsersId");

			migrationBuilder.AddForeignKey(
				name: "FK_Comments_Posts_PostId",
				table: "Comments",
				column: "PostId",
				principalTable: "Posts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Comments_Posts_PostId",
				table: "Comments");

			migrationBuilder.DropTable(
				name: "LanguageUser");

			migrationBuilder.DropTable(
				name: "Posts");

			migrationBuilder.DropTable(
				name: "TechnologyUser");

			migrationBuilder.DropIndex(
				name: "IX_Comments_PostId",
				table: "Comments");

			migrationBuilder.DropColumn(
				name: "PostId",
				table: "Comments");

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
	}
}

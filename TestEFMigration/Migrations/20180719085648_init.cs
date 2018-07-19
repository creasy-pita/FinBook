using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestEFMigration.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectRecommends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Company = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    FinStage = table.Column<string>(nullable: true),
                    FromUserAvatar = table.Column<string>(nullable: true),
                    FromUserId = table.Column<int>(nullable: false),
                    FromUserName = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    ProjectAvatar = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    RecommednType = table.Column<int>(nullable: false),
                    RecommendTime = table.Column<DateTime>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRecommends", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRecommends");
        }
    }
}

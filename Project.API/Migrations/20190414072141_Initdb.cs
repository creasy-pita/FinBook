using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Infrastructure.Migrations
{
    public partial class Initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    OriginBPFile = table.Column<string>(nullable: true),
                    FormatBPFile = table.Column<string>(nullable: true),
                    ShowSecurityInfo = table.Column<short>(nullable: false),
                    ProvinceId = table.Column<string>(nullable: true),
                    ProvinceName = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(nullable: true),
                    AreaName = table.Column<string>(nullable: true),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    FinPercentage = table.Column<int>(nullable: false),
                    FinStage = table.Column<int>(nullable: false),
                    FinMoney = table.Column<string>(nullable: true),
                    Income = table.Column<int>(nullable: false),
                    Revenue = table.Column<int>(nullable: false),
                    Valuation = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    BrokerageOption = table.Column<string>(nullable: true),
                    SourceId = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectContributors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    IsCloser = table.Column<short>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ContributorType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContributors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectContributors_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProperties",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Value = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProperties", x => new { x.Key, x.ProjectId, x.Value });
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectViewers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectViewers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectVisibleRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(nullable: false),
                    Visible = table.Column<short>(nullable: false),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectVisibleRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectVisibleRules_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectContributors_ProjectId",
                table: "ProjectContributors",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_ProjectId",
                table: "ProjectProperties",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectViewers_ProjectId",
                table: "ProjectViewers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectVisibleRules_ProjectId",
                table: "ProjectVisibleRules",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectContributors");

            migrationBuilder.DropTable(
                name: "ProjectProperties");

            migrationBuilder.DropTable(
                name: "ProjectViewers");

            migrationBuilder.DropTable(
                name: "ProjectVisibleRules");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}

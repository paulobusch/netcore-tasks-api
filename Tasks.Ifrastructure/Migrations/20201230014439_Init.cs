using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Ifrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Login = table.Column<string>(maxLength: 150, nullable: false),
                    CPF = table.Column<string>(maxLength: 11, nullable: false),
                    Password = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeveloperProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeveloperId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeveloperProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeveloperProjects_Developers_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Developers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeveloperProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDeveloperProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeveloperProjectId = table.Column<Guid>(nullable: false),
                    WorkId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDeveloperProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkDeveloperProjects_DeveloperProjects_DeveloperProjectId",
                        column: x => x.DeveloperProjectId,
                        principalTable: "DeveloperProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkDeveloperProjects_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeveloperProjects_DeveloperId",
                table: "DeveloperProjects",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_DeveloperProjects_ProjectId_DeveloperId",
                table: "DeveloperProjects",
                columns: new[] { "ProjectId", "DeveloperId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Developers_Login",
                table: "Developers",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkDeveloperProjects_WorkId",
                table: "WorkDeveloperProjects",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDeveloperProjects_DeveloperProjectId_WorkId",
                table: "WorkDeveloperProjects",
                columns: new[] { "DeveloperProjectId", "WorkId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Works_EndTime",
                table: "Works",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Works_StartTime",
                table: "Works",
                column: "StartTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkDeveloperProjects");

            migrationBuilder.DropTable(
                name: "DeveloperProjects");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}

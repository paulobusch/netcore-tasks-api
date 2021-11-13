using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Ifrastructure.Migrations
{
    public partial class ChangeWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkDeveloperProjects");

            migrationBuilder.DropIndex(
                name: "IX_Works_EndTime",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_StartTime",
                table: "Works");

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperProjectId",
                table: "Works",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Works",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Works_DeveloperProjectId",
                table: "Works",
                column: "DeveloperProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_DeveloperProjects_DeveloperProjectId",
                table: "Works",
                column: "DeveloperProjectId",
                principalTable: "DeveloperProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_DeveloperProjects_DeveloperProjectId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_DeveloperProjectId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "DeveloperProjectId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Works");

            migrationBuilder.CreateTable(
                name: "WorkDeveloperProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    DeveloperProjectId = table.Column<Guid>(type: "char(36)", nullable: false),
                    WorkId = table.Column<Guid>(type: "char(36)", nullable: false)
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
                name: "IX_Works_EndTime",
                table: "Works",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Works_StartTime",
                table: "Works",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDeveloperProjects_WorkId",
                table: "WorkDeveloperProjects",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDeveloperProjects_DeveloperProjectId_WorkId",
                table: "WorkDeveloperProjects",
                columns: new[] { "DeveloperProjectId", "WorkId" },
                unique: true);
        }
    }
}

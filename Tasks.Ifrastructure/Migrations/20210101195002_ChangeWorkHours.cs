using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Ifrastructure.Migrations
{
    public partial class ChangeWorkHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Hours",
                table: "Works",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "Works",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Ifrastructure.Migrations
{
    public partial class ChangeDeveloper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Developers");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Developers",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Developers");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Developers",
                type: "varchar(80) CHARACTER SET utf8mb4",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}

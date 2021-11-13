using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Ifrastructure.Migrations
{
    public partial class DeveloperSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Developers",
                columns: new[] { "Id", "CPF", "Login", "Name", "PasswordHash" },
                values: new object[] { new Guid("86b6b3a7-965e-46dd-843d-661f6e76ded1"), "13467669085", "pleno", "Pleno", "1VPRSEeaJokUzst3sriOag==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Developers",
                keyColumn: "Id",
                keyValue: new Guid("86b6b3a7-965e-46dd-843d-661f6e76ded1"));
        }
    }
}

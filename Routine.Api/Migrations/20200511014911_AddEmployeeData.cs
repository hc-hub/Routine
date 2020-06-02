using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class AddEmployeeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("b5187888-1e98-4ded-9a74-0282fa736d5d"), new Guid("2c4ecd04-5bae-4e20-8125-c5a26aac5c0a"), new DateTime(1985, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "001", "Li", 1, "Guangzhu" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: new Guid("b5187888-1e98-4ded-9a74-0282fa736d5d"));
        }
    }
}

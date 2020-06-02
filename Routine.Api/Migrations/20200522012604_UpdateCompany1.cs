using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class UpdateCompany1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

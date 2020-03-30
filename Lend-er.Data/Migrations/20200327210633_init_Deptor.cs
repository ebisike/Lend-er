using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend_er.Data.Migrations
{
    public partial class init_Deptor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaid",
                table: "deptors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "deptors",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "percentageInterest",
                table: "deptors",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePaid",
                table: "deptors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "deptors");

            migrationBuilder.DropColumn(
                name: "percentageInterest",
                table: "deptors");
        }
    }
}

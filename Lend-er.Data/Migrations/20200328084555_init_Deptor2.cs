using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend_er.Data.Migrations
{
    public partial class init_Deptor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "deptId",
                table: "payments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deptId",
                table: "payments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend_er.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "percentageInterest",
                table: "creditors",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percentageInterest",
                table: "creditors");
        }
    }
}

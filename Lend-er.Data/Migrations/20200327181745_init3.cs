using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend_er.Data.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "creditors");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "creditors");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "creditors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "creditors");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "creditors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "creditors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend_er.Data.Migrations
{
    public partial class init_Deptor3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "deptorsId",
                table: "payments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_deptorsId",
                table: "payments",
                column: "deptorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_deptors_deptorsId",
                table: "payments",
                column: "deptorsId",
                principalTable: "deptors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_deptors_deptorsId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_deptorsId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "deptorsId",
                table: "payments");
        }
    }
}

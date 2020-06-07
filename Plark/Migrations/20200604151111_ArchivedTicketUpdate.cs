using Microsoft.EntityFrameworkCore.Migrations;

namespace Plark.Migrations
{
    public partial class ArchivedTicketUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedTickets_Cars_CarId",
                table: "ArchivedTickets");

            migrationBuilder.DropIndex(
                name: "IX_ArchivedTickets_CarId",
                table: "ArchivedTickets");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "ArchivedTickets");

            migrationBuilder.AddColumn<string>(
                name: "CarNumberPlate",
                table: "ArchivedTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarNumberPlate",
                table: "ArchivedTickets");

            migrationBuilder.AddColumn<long>(
                name: "CarId",
                table: "ArchivedTickets",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedTickets_CarId",
                table: "ArchivedTickets",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedTickets_Cars_CarId",
                table: "ArchivedTickets",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

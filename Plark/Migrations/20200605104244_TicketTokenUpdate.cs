using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Plark.Migrations
{
    public partial class TicketTokenUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cars_CarId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CarId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CarNumberPlate",
                table: "ArchivedTickets");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "ArchivedTickets");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ArchivedTickets");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "ArchivedTickets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ArchivedTickets");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ArchivedTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "ArchivedTickets");

            migrationBuilder.AddColumn<long>(
                name: "CarId",
                table: "Tickets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Closed",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CarNumberPlate",
                table: "ArchivedTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Closed",
                table: "ArchivedTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ArchivedTickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "ArchivedTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ArchivedTickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CarId",
                table: "Tickets",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cars_CarId",
                table: "Tickets",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

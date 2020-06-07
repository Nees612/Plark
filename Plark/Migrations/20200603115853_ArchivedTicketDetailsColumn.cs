using Microsoft.EntityFrameworkCore.Migrations;

namespace Plark.Migrations
{
    public partial class ArchivedTicketDetailsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "ArchivedTickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "ArchivedTickets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Plark.Migrations
{
    public partial class SignalR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CarId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ParkingTimeInHours",
                table: "Tickets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ConnectionIds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubConnectionId = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wardens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    WorkPlace = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wardens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CarId",
                table: "Tickets",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionIds_UserId",
                table: "ConnectionIds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cars_CarId",
                table: "Tickets",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cars_CarId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "ConnectionIds");

            migrationBuilder.DropTable(
                name: "Wardens");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CarId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ParkingTimeInHours",
                table: "Tickets");
        }
    }
}

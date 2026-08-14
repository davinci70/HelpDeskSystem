using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserRelationsFromTicketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AssignedByUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ResolvedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ResolvedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedByUserId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolvedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedByUserId",
                table: "Tickets",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AssignedByUserId",
                table: "Tickets",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

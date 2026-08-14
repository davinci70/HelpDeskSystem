using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveActionsColumnsFromTicketEntityAndAddOneActionByUserIdToTicketHistoryEntity : Migration
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

            migrationBuilder.DropColumn(
                name: "ReopenedByUserId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "ActionByUserId",
                table: "TicketHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_ActionByUserId",
                table: "TicketHistories",
                column: "ActionByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_Users_ActionByUserId",
                table: "TicketHistories",
                column: "ActionByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_Users_ActionByUserId",
                table: "TicketHistories");

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
                name: "IX_TicketHistories_ActionByUserId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "ActionByUserId",
                table: "TicketHistories");

            migrationBuilder.AddColumn<int>(
                name: "ReopenedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AssignedByUserId",
                table: "Tickets",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

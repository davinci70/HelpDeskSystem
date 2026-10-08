using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketCommentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketCommentId",
                table: "TicketHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketCommentId",
                table: "TicketHistories",
                column: "TicketCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_TicketComments_TicketCommentId",
                table: "TicketHistories",
                column: "TicketCommentId",
                principalTable: "TicketComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_TicketComments_TicketCommentId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketCommentId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "TicketCommentId",
                table: "TicketHistories");
        }
    }
}

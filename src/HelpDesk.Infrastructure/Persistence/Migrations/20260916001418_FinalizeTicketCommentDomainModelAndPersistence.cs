using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeTicketCommentDomainModelAndPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TicketComments",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "TicketComments",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TicketComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "TicketComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInternal",
                table: "TicketComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "TicketComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_CreatedByUserId",
                table: "TicketComments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_DeletedByUserId",
                table: "TicketComments",
                column: "DeletedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Users_CreatedByUserId",
                table: "TicketComments",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Users_DeletedByUserId",
                table: "TicketComments",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Users_CreatedByUserId",
                table: "TicketComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Users_DeletedByUserId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_CreatedByUserId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_DeletedByUserId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "IsInternal",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "TicketComments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "TicketComments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}

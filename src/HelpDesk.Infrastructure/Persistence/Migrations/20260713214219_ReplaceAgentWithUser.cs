using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceAgentWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Agents_AssigneeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AssigneeId",
                table: "Tickets",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AssigneeId",
                table: "Tickets");

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Agents_AssigneeId",
                table: "Tickets",
                column: "AssigneeId",
                principalTable: "Agents",
                principalColumn: "Id");
        }
    }
}

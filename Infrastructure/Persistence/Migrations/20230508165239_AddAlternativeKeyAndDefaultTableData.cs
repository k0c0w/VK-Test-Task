using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAlternativeKeyAndDefaultTableData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_user_state_code",
                table: "user_state",
                column: "code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_user_group_code",
                table: "user_group",
                column: "code");

            migrationBuilder.InsertData(
                table: "user_group",
                columns: new[] { "id", "code", "description" },
                values: new object[,]
                {
                    { 1, "Admin", "Main hero of database." },
                    { 2, "User", "Just a user." }
                });

            migrationBuilder.InsertData(
                table: "user_state",
                columns: new[] { "id", "code", "description" },
                values: new object[,]
                {
                    { 1, "Active", "User can access service." },
                    { 2, "Blocked", "Administratively deleted." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_user_state_code",
                table: "user_state");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_user_group_code",
                table: "user_group");

            migrationBuilder.DeleteData(
                table: "user_group",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_group",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "user_state",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_state",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}

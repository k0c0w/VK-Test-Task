using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UniqueLoginAndRenamedForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_user_group_GroupId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_state_StateId",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "user",
                newName: "user_state_id");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "user",
                newName: "user_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_StateId",
                table: "user",
                newName: "IX_user_user_state_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_GroupId",
                table: "user",
                newName: "IX_user_user_group_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "user",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_user_login",
                table: "user",
                column: "login");

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_group_user_group_id",
                table: "user",
                column: "user_group_id",
                principalTable: "user_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_state_user_state_id",
                table: "user",
                column: "user_state_id",
                principalTable: "user_state",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_user_group_user_group_id",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_state_user_state_id",
                table: "user");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_user_login",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "user_state_id",
                table: "user",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "user_group_id",
                table: "user",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_user_user_state_id",
                table: "user",
                newName: "IX_user_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_user_user_group_id",
                table: "user",
                newName: "IX_user_GroupId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_group_GroupId",
                table: "user",
                column: "GroupId",
                principalTable: "user_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_state_StateId",
                table: "user",
                column: "StateId",
                principalTable: "user_state",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

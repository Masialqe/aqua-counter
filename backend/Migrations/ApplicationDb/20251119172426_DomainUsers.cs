using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AC.Backend.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class DomainUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "application");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "application");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UserGroups",
                newSchema: "application");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Groups",
                newSchema: "application");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Addresses",
                newSchema: "application");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentityId",
                schema: "application",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                schema: "application",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                schema: "application",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "application",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                schema: "application",
                newName: "UserGroups");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "application",
                newName: "Groups");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "application",
                newName: "Addresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserIdentityId",
                table: "Users",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}

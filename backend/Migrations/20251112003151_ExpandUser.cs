using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AC.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ExpandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGivenName",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserSurname",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressStreet = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AddressCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AddressPostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AddressBuildingNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AddressApartmentNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserGivenName",
                table: "AspNetUsers",
                column: "UserGivenName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserSurname",
                table: "AspNetUsers",
                column: "UserSurname");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_ApplicationUserId",
                table: "UserAddresses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAddresses_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAddresses_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserGivenName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSurname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserGivenName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSurname",
                table: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AC.Backend.Migrations
{
    /// <inheritdoc />
    public partial class IdentityBaseConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DeviceIpAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DeviceAgent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DeviceLastUsedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeviceCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_UserDevices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenValue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RefreshTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserDeviceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_UserDevices_UserDeviceId",
                        column: x => x.UserDeviceId,
                        principalTable: "UserDevices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_RefreshTokenUser",
                table: "RefreshTokens",
                column: "RefreshTokenUser");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_RefreshTokenValue",
                table: "RefreshTokens",
                column: "RefreshTokenValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserDeviceId",
                table: "RefreshTokens",
                column: "UserDeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_DeviceAgent",
                table: "UserDevices",
                column: "DeviceAgent");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_DeviceIpAddress",
                table: "UserDevices",
                column: "DeviceIpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId_DeviceIpAddress_DeviceAgent",
                table: "UserDevices",
                columns: new[] { "UserId", "DeviceIpAddress", "DeviceAgent" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserDevices");
        }
    }
}

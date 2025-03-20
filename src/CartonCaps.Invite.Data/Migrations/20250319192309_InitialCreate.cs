using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CartonCaps.Invite.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferralSource",
                columns: table => new
                {
                    ReferralSourceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralSource", x => x.ReferralSourceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ReferralCodes",
                columns: table => new
                {
                    ReferralCodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralCodes", x => x.ReferralCodeId);
                    table.ForeignKey(
                        name: "FK_ReferralCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Redemptions",
                columns: table => new
                {
                    RedemptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RedeemerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferrerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferralCodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferralSourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    RedeemedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redemptions", x => x.RedemptionId);
                    table.ForeignKey(
                        name: "FK_Redemptions_ReferralCodes_ReferralCodeId",
                        column: x => x.ReferralCodeId,
                        principalTable: "ReferralCodes",
                        principalColumn: "ReferralCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redemptions_ReferralSource_ReferralSourceId",
                        column: x => x.ReferralSourceId,
                        principalTable: "ReferralSource",
                        principalColumn: "ReferralSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Redemptions_Users_RedeemerId",
                        column: x => x.RedeemerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Redemptions_Users_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    ReferralId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferrerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferralCodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferralSourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.ReferralId);
                    table.ForeignKey(
                        name: "FK_Referrals_ReferralCodes_ReferralCodeId",
                        column: x => x.ReferralCodeId,
                        principalTable: "ReferralCodes",
                        principalColumn: "ReferralCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Referrals_ReferralSource_ReferralSourceId",
                        column: x => x.ReferralSourceId,
                        principalTable: "ReferralSource",
                        principalColumn: "ReferralSourceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Referrals_Users_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ReferralSource",
                columns: new[] { "ReferralSourceId", "SourceName" },
                values: new object[,]
                {
                    { 1, "Android" },
                    { 2, "iOS" },
                    { 3, "Chrome" },
                    { 4, "Edge" },
                    { 5, "Firefox" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Redemptions_RedeemerId_ReferralCodeId",
                table: "Redemptions",
                columns: new[] { "RedeemerId", "ReferralCodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Redemptions_ReferralCodeId",
                table: "Redemptions",
                column: "ReferralCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Redemptions_ReferralSourceId",
                table: "Redemptions",
                column: "ReferralSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Redemptions_ReferrerId",
                table: "Redemptions",
                column: "ReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralCodes_Code",
                table: "ReferralCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralCodes_UserId",
                table: "ReferralCodes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferralCodeId",
                table: "Referrals",
                column: "ReferralCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferralSourceId",
                table: "Referrals",
                column: "ReferralSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferrerId",
                table: "Referrals",
                column: "ReferrerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Redemptions");

            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropTable(
                name: "ReferralCodes");

            migrationBuilder.DropTable(
                name: "ReferralSource");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API_DbAccess.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SentencesOfTheDay",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sentence = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentencesOfTheDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    UserAddress = table.Column<string>(maxLength: 50, nullable: false),
                    LoyaltyPoints = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dress",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DressName = table.Column<string>(maxLength: 50, nullable: false),
                    Describe = table.Column<string>(maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    DateBeginAvailable = table.Column<DateTime>(nullable: false),
                    DateEndAvailable = table.Column<DateTime>(nullable: false),
                    UrlImage = table.Column<string>(maxLength: 255, nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dress_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressOrder",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BillingDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    BillingAddress = table.Column<string>(maxLength: 50, nullable: false),
                    DeliveryAddress = table.Column<string>(maxLength: 50, nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DressOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    DressId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Dress_DressId",
                        column: x => x.DressId,
                        principalTable: "Dress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OrderLine",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateBeginLocation = table.Column<DateTime>(nullable: false),
                    DateEndLocation = table.Column<DateTime>(nullable: false),
                    FinalPrice = table.Column<decimal>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    DressOrderId = table.Column<string>(nullable: false),
                    DressId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLine_Dress_DressId",
                        column: x => x.DressId,
                        principalTable: "Dress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLine_DressOrder_DressOrderId",
                        column: x => x.DressOrderId,
                        principalTable: "DressOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dress_UserId",
                table: "Dress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DressOrder_UserId",
                table: "DressOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_DressId",
                table: "Favorites",
                column: "DressId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_DressId",
                table: "OrderLine",
                column: "DressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_DressOrderId",
                table: "OrderLine",
                column: "DressOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OrderLine");

            migrationBuilder.DropTable(
                name: "SentencesOfTheDay");

            migrationBuilder.DropTable(
                name: "Dress");

            migrationBuilder.DropTable(
                name: "DressOrder");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

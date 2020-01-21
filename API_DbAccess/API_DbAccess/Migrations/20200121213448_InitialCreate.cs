using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API_DbAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dress_AspNetUsers_UserId",
                table: "Dress");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrderLine",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "DressOrder",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Dress",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PartnerId",
                table: "Dress",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Dress",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AspNetUsers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dress_AspNetUsers_UserId",
                table: "Dress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dress_AspNetUsers_UserId",
                table: "Dress");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrderLine");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "DressOrder");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Dress");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Dress");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Dress",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dress_AspNetUsers_UserId",
                table: "Dress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

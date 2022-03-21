using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShop.Data.Migrations
{
    public partial class updatecartcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AppUser_AppUserId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_AppUserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Carts_AppUserId",
                table: "Carts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AppUser_AppUserId",
                table: "Carts",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

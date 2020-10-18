using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace main_service.Migrations
{
    public partial class baseentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserAuth",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "UserAuth",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserAuth");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "UserAuth");
        }
    }
}

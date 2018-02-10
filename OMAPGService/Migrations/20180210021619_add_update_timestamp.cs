using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class add_update_timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Devices",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Devices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "NOW()");
        }
    }
}

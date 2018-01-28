using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class add_timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "form",
                table: "Pokemon",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "timestamp",
                table: "Pokemon",
                type: "int8",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "form",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "Pokemon");
        }
    }
}

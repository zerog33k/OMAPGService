using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class add_ignore_list : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IgnorePokemonStr",
                table: "Devices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxDistance",
                table: "Devices",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinLevelAlert",
                table: "Devices",
                type: "int4",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgnorePokemonStr",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MaxDistance",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MinLevelAlert",
                table: "Devices");
        }
    }
}

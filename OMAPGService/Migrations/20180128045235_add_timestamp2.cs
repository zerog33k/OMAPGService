using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class add_timestamp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "form",
                table: "Pokemon",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "form",
                table: "Pokemon",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);
        }
    }
}

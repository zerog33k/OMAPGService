using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class addServiceData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(127)", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true),
                    is_in_battle = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<long>(type: "bigint", nullable: false),
                    lat = table.Column<double>(type: "double", nullable: false),
                    lon = table.Column<double>(type: "double", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    pokemon_id = table.Column<int>(type: "int", nullable: false),
                    pokemon_name = table.Column<string>(type: "longtext", nullable: true),
                    sigting_id = table.Column<int>(type: "int", nullable: false),
                    slots_available = table.Column<int>(type: "int", nullable: true),
                    team = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemon",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(127)", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    atk = table.Column<int>(type: "int", nullable: false),
                    cp = table.Column<int>(type: "int", nullable: false),
                    damage1 = table.Column<string>(type: "longtext", nullable: true),
                    damage2 = table.Column<string>(type: "longtext", nullable: true),
                    def = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    lat = table.Column<double>(type: "double", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    lon = table.Column<double>(type: "double", nullable: false),
                    move1 = table.Column<string>(type: "longtext", nullable: true),
                    move2 = table.Column<string>(type: "longtext", nullable: true),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    pokemon_id = table.Column<int>(type: "int", nullable: false),
                    sta = table.Column<int>(type: "int", nullable: false),
                    trash = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Raids",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(127)", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    cp = table.Column<int>(type: "int", nullable: false),
                    lat = table.Column<double>(type: "double", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    lon = table.Column<double>(type: "double", nullable: false),
                    move_1 = table.Column<string>(type: "longtext", nullable: true),
                    move_2 = table.Column<string>(type: "longtext", nullable: true),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    pokemon_id = table.Column<int>(type: "int", nullable: false),
                    pokemon_name = table.Column<string>(type: "longtext", nullable: true),
                    team = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raids", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "Pokemon");

            migrationBuilder.DropTable(
                name: "Raids");
        }
    }
}

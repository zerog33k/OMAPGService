using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OMAPGService.Migrations
{
    public partial class upadateDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    DistanceAlert = table.Column<int>(type: "int4", nullable: false),
                    LocationLat = table.Column<double>(type: "float8", nullable: false),
                    LocationLon = table.Column<double>(type: "float8", nullable: false),
                    NotifyEnabled = table.Column<bool>(type: "bool", nullable: false),
                    NotifyPokemonStr = table.Column<string>(type: "text", nullable: true),
                    OSType = table.Column<int>(type: "int4", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "timestamp", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_in_battle = table.Column<bool>(type: "bool", nullable: false),
                    last_modified = table.Column<long>(type: "int8", nullable: false),
                    lat = table.Column<double>(type: "float8", nullable: false),
                    lon = table.Column<double>(type: "float8", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    pokemon_id = table.Column<int>(type: "int4", nullable: false),
                    pokemon_name = table.Column<string>(type: "text", nullable: true),
                    sigting_id = table.Column<int>(type: "int4", nullable: false),
                    slots_available = table.Column<int>(type: "int4", nullable: true),
                    team = table.Column<int>(type: "int4", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemon",
                columns: table => new
                {
                    idValue = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "timestamp", nullable: false),
                    atk = table.Column<int>(type: "int4", nullable: false),
                    cp = table.Column<int>(type: "int4", nullable: false),
                    damage1 = table.Column<string>(type: "text", nullable: true),
                    damage2 = table.Column<string>(type: "text", nullable: true),
                    def = table.Column<int>(type: "int4", nullable: false),
                    expires_at = table.Column<long>(type: "int8", nullable: false),
                    gender = table.Column<int>(type: "int4", nullable: false),
                    id = table.Column<string>(type: "text", nullable: true),
                    lat = table.Column<double>(type: "float8", nullable: false),
                    level = table.Column<int>(type: "int4", nullable: false),
                    lon = table.Column<double>(type: "float8", nullable: false),
                    move1 = table.Column<string>(type: "text", nullable: true),
                    move2 = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    pokemon_id = table.Column<int>(type: "int4", nullable: false),
                    sta = table.Column<int>(type: "int4", nullable: false),
                    trash = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.idValue);
                });

            migrationBuilder.CreateTable(
                name: "Raids",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(type: "timestamp", nullable: false),
                    cp = table.Column<int>(type: "int4", nullable: false),
                    lat = table.Column<double>(type: "float8", nullable: false),
                    level = table.Column<int>(type: "int4", nullable: false),
                    lon = table.Column<double>(type: "float8", nullable: false),
                    move_1 = table.Column<string>(type: "text", nullable: true),
                    move_2 = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    pokemon_id = table.Column<int>(type: "int4", nullable: false),
                    pokemon_name = table.Column<string>(type: "text", nullable: true),
                    team = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raids", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotifyId = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DeviceId = table.Column<long>(type: "int8", nullable: true),
                    Distance = table.Column<double>(type: "float8", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    PokemonId = table.Column<int>(type: "int4", nullable: false),
                    seen = table.Column<bool>(type: "bool", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotifyId);
                    table.ForeignKey(
                        name: "FK_Notifications_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeviceId",
                table: "Notifications",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Pokemon");

            migrationBuilder.DropTable(
                name: "Raids");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}

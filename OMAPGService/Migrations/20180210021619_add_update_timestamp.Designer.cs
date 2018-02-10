﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using OMAPGServiceData.Models;
using System;

namespace OMAPGService.Migrations
{
    [DbContext(typeof(OMAPGContext))]
    [Migration("20180210021619_add_update_timestamp")]
    partial class add_update_timestamp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("OMAPGServiceData.Models.Device", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("DeviceId");

                    b.Property<int>("DistanceAlert");

                    b.Property<string>("IgnorePokemonStr");

                    b.Property<double>("LocationLat");

                    b.Property<double>("LocationLon");

                    b.Property<int>("MaxDistance");

                    b.Property<int>("MinLevelAlert");

                    b.Property<bool>("Notify100");

                    b.Property<bool>("Notify90");

                    b.Property<bool>("NotifyEnabled");

                    b.Property<string>("NotifyPokemonStr");

                    b.Property<int>("OSType");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("OMAPGServiceData.Models.Gym", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("description");

                    b.Property<bool>("is_in_battle");

                    b.Property<long>("last_modified");

                    b.Property<double>("lat");

                    b.Property<double>("lon");

                    b.Property<string>("name");

                    b.Property<int>("pokemon_id");

                    b.Property<string>("pokemon_name");

                    b.Property<int>("sigting_id");

                    b.Property<int?>("slots_available");

                    b.Property<int>("team");

                    b.Property<string>("url");

                    b.HasKey("id");

                    b.ToTable("Gyms");
                });

            modelBuilder.Entity("OMAPGServiceData.Models.Notification", b =>
                {
                    b.Property<long>("NotifyId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NOW()");

                    b.Property<long?>("DeviceId");

                    b.Property<double>("Distance");

                    b.Property<string>("Message");

                    b.Property<int>("PokemonId");

                    b.Property<bool>("seen");

                    b.HasKey("NotifyId");

                    b.HasIndex("DeviceId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("OMAPGServiceData.Models.Pokemon", b =>
                {
                    b.Property<int>("idValue")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("atk");

                    b.Property<int>("cp");

                    b.Property<string>("damage1");

                    b.Property<string>("damage2");

                    b.Property<int>("def");

                    b.Property<long>("expires_at");

                    b.Property<int?>("form");

                    b.Property<int>("gender");

                    b.Property<string>("id");

                    b.Property<double>("lat");

                    b.Property<int>("level");

                    b.Property<double>("lon");

                    b.Property<string>("move1");

                    b.Property<string>("move2");

                    b.Property<string>("name");

                    b.Property<int>("pokemon_id");

                    b.Property<int>("sta");

                    b.Property<long>("timestamp");

                    b.Property<bool>("trash");

                    b.HasKey("idValue");

                    b.ToTable("Pokemon");
                });

            modelBuilder.Entity("OMAPGServiceData.Models.Raid", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("cp");

                    b.Property<double>("lat");

                    b.Property<int>("level");

                    b.Property<double>("lon");

                    b.Property<string>("move_1");

                    b.Property<string>("move_2");

                    b.Property<string>("name");

                    b.Property<int>("pokemon_id");

                    b.Property<string>("pokemon_name");

                    b.Property<int>("team");

                    b.HasKey("id");

                    b.ToTable("Raids");
                });

            modelBuilder.Entity("OMAPGServiceData.Models.Notification", b =>
                {
                    b.HasOne("OMAPGServiceData.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId");
                });
#pragma warning restore 612, 618
        }
    }
}

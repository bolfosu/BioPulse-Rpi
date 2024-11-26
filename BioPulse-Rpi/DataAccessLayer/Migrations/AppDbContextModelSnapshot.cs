﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("DataAccessLayer.Models.EcSensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWireless")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LastReading")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("LastReadingTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EcSensors");
                });

            modelBuilder.Entity("DataAccessLayer.Models.ImageCapture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ImageCaptures");
                });

            modelBuilder.Entity("DataAccessLayer.Models.LightSensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LightSensors");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PhSensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWireless")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LastReading")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("LastReadingTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PhSensors");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PlantProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("EcMax")
                        .HasColumnType("REAL");

                    b.Property<double>("EcMin")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("LightOffTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("LightOnTime")
                        .HasColumnType("TEXT");

                    b.Property<double>("LuxMax")
                        .HasColumnType("REAL");

                    b.Property<double>("LuxMin")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PhMax")
                        .HasColumnType("REAL");

                    b.Property<double>("PhMin")
                        .HasColumnType("REAL");

                    b.Property<double>("TemperatureMax")
                        .HasColumnType("REAL");

                    b.Property<double>("TemperatureMin")
                        .HasColumnType("REAL");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PlantProfiles");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TemperatureSensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWireless")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LastReading")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("LastReadingTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemperatureSensors");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityAnswerHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityQuestion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PlantProfile", b =>
                {
                    b.HasOne("DataAccessLayer.Models.User", null)
                        .WithMany("PlantProfiles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.Navigation("PlantProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}

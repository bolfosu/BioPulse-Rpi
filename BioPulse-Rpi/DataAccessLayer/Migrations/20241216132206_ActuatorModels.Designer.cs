﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241216132206_ActuatorModels")]
    partial class ActuatorModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

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

                    b.Property<double>("LightMax")
                        .HasColumnType("REAL");

                    b.Property<double>("LightMin")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("LightOffTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LightOnTime")
                        .HasColumnType("TEXT");

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

                    b.HasKey("Id");

                    b.ToTable("PlantProfiles");
                });

            modelBuilder.Entity("DataAccessLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSensorNotChangingNotificationEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSensorOffNotificationEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWaterLevelLowNotificationEnabled")
                        .HasColumnType("INTEGER");

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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConnectionDetails")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalSensorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SensorType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SensorType");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("SensorReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SensorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.HasIndex("Timestamp");

                    b.ToTable("SensorReadings");
                });

            modelBuilder.Entity("SensorReading", b =>
                {
                    b.HasOne("Sensor", "Sensor")
                        .WithMany("SensorReadings")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("Sensor", b =>
                {
                    b.Navigation("SensorReadings");
                });
#pragma warning restore 612, 618
        }
    }
}

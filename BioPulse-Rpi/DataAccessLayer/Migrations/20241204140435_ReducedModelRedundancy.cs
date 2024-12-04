using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ReducedModelRedundancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EcSensors");

            migrationBuilder.DropTable(
                name: "LightSensors");

            migrationBuilder.DropTable(
                name: "PhSensors");

            migrationBuilder.DropTable(
                name: "TemperatureSensors");

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWireless = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastReading = table.Column<double>(type: "REAL", nullable: false),
                    LastReadingTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Address = table.Column<byte>(type: "INTEGER", nullable: false),
                    SensorType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.CreateTable(
                name: "EcSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWireless = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastReading = table.Column<double>(type: "REAL", nullable: false),
                    LastReadingTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWireless = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastReading = table.Column<double>(type: "REAL", nullable: false),
                    LastReadingTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsWireless = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastReading = table.Column<double>(type: "REAL", nullable: false),
                    LastReadingTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureSensors", x => x.Id);
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddLightMinMaxToPlantProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LuxMin",
                table: "PlantProfiles",
                newName: "LightMin");

            migrationBuilder.RenameColumn(
                name: "LuxMax",
                table: "PlantProfiles",
                newName: "LightMax");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LightMin",
                table: "PlantProfiles",
                newName: "LuxMin");

            migrationBuilder.RenameColumn(
                name: "LightMax",
                table: "PlantProfiles",
                newName: "LuxMax");
        }
    }
}

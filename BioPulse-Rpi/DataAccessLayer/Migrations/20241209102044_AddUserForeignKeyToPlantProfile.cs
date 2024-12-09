using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserForeignKeyToPlantProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantProfiles_Users_UserId",
                table: "PlantProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PlantProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantProfiles_Users_UserId",
                table: "PlantProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantProfiles_Users_UserId",
                table: "PlantProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PlantProfiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantProfiles_Users_UserId",
                table: "PlantProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

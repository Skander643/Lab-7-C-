using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardData.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Sensors",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Sensors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

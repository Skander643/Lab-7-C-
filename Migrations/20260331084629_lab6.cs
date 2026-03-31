using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardData.Migrations
{
    /// <inheritdoc />
    public partial class lab6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "SensorValueHistories",
                newName: "MeasuredValue");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "SensorValueHistories",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MeasuredValue",
                table: "SensorValueHistories",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "SensorValueHistories",
                newName: "Timestamp");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePumpTankSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PumpTank",
                columns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 3, 3 },
                    { 3, 4 },
                    { 4, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 4, 4 });
        }
    }
}

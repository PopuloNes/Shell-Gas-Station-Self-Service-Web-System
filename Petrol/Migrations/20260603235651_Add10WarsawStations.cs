using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class Add10WarsawStations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GasStations",
                columns: new[] { "Id", "Address", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 3, "Wolska 15, 01-201 Warszawa", 52.233499999999999, 20.970199999999998, "Orlen Warszawa Wola" },
                    { 4, "Puławska 427, 02-801 Warszawa", 52.148099999999999, 21.020499999999998, "BP Warszawa Mokotów" },
                    { 5, "KEN 98, 02-777 Warszawa", 52.152799999999999, 21.045000000000002, "Circle K Warszawa Ursynów" },
                    { 6, "Kasprowicza 117, 01-949 Warszawa", 52.285899999999998, 20.938800000000001, "Lotos Warszawa Bielany" },
                    { 7, "Grójecka 100, 02-389 Warszawa", 52.2104, 20.975000000000001, "Shell Warszawa Ochota" },
                    { 8, "Mickiewicza 20, 01-517 Warszawa", 52.2682, 20.985600000000002, "Orlen Warszawa Żoliborz" },
                    { 9, "Radzyminska 200, 03-674 Warszawa", 52.274099999999997, 21.074300000000001, "Amic Warszawa Targówek" },
                    { 10, "Patriotów 100, 04-944 Warszawa", 52.180199999999999, 21.189499999999999, "Moya Warszawa Wawer" },
                    { 11, "Krakowska 200, 02-219 Warszawa", 52.183399999999999, 20.941600000000001, "Circle K Warszawa Włochy" },
                    { 12, "Górczewska 212, 01-460 Warszawa", 52.2393, 20.916499999999999, "BP Warszawa Bemowo" }
                });

            migrationBuilder.InsertData(
                table: "Pumps",
                columns: new[] { "Id", "GasStationId", "Name", "Status" },
                values: new object[,]
                {
                    { 5, 3, "Pump 1", 2 },
                    { 6, 3, "Pump 2", 2 },
                    { 7, 3, "Pump 3", 2 },
                    { 8, 3, "Pump 4", 2 },
                    { 9, 4, "Pump 1", 2 },
                    { 10, 4, "Pump 2", 2 },
                    { 11, 4, "Pump 3", 2 },
                    { 12, 4, "Pump 4", 2 },
                    { 13, 5, "Pump 1", 2 },
                    { 14, 5, "Pump 2", 2 },
                    { 15, 5, "Pump 3", 2 },
                    { 16, 5, "Pump 4", 2 },
                    { 17, 6, "Pump 1", 2 },
                    { 18, 6, "Pump 2", 2 },
                    { 19, 6, "Pump 3", 2 },
                    { 20, 6, "Pump 4", 2 },
                    { 21, 7, "Pump 1", 2 },
                    { 22, 7, "Pump 2", 2 },
                    { 23, 7, "Pump 3", 2 },
                    { 24, 7, "Pump 4", 2 },
                    { 25, 8, "Pump 1", 2 },
                    { 26, 8, "Pump 2", 2 },
                    { 27, 8, "Pump 3", 2 },
                    { 28, 8, "Pump 4", 2 },
                    { 29, 9, "Pump 1", 2 },
                    { 30, 9, "Pump 2", 2 },
                    { 31, 9, "Pump 3", 2 },
                    { 32, 9, "Pump 4", 2 },
                    { 33, 10, "Pump 1", 2 },
                    { 34, 10, "Pump 2", 2 },
                    { 35, 10, "Pump 3", 2 },
                    { 36, 10, "Pump 4", 2 },
                    { 37, 11, "Pump 1", 2 },
                    { 38, 11, "Pump 2", 2 },
                    { 39, 11, "Pump 3", 2 },
                    { 40, 11, "Pump 4", 2 },
                    { 41, 12, "Pump 1", 2 },
                    { 42, 12, "Pump 2", 2 },
                    { 43, 12, "Pump 3", 2 },
                    { 44, 12, "Pump 4", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tanks",
                columns: new[] { "Id", "Capacity", "FuelTypeId", "GasStationId", "Volume" },
                values: new object[,]
                {
                    { 5, 10000.0, 1, 3, 5000.0 },
                    { 6, 10000.0, 2, 3, 5000.0 },
                    { 7, 10000.0, 3, 3, 5000.0 },
                    { 8, 10000.0, 4, 3, 5000.0 },
                    { 9, 10000.0, 1, 4, 5000.0 },
                    { 10, 10000.0, 3, 4, 5000.0 },
                    { 11, 10000.0, 1, 5, 5000.0 },
                    { 12, 10000.0, 3, 5, 5000.0 },
                    { 13, 10000.0, 4, 5, 5000.0 },
                    { 14, 10000.0, 1, 6, 5000.0 },
                    { 15, 10000.0, 2, 6, 5000.0 },
                    { 16, 10000.0, 3, 6, 5000.0 },
                    { 17, 10000.0, 4, 6, 5000.0 },
                    { 18, 10000.0, 1, 7, 5000.0 },
                    { 19, 10000.0, 4, 7, 5000.0 },
                    { 20, 10000.0, 3, 8, 5000.0 },
                    { 21, 10000.0, 1, 9, 5000.0 },
                    { 22, 10000.0, 2, 9, 5000.0 },
                    { 23, 10000.0, 3, 9, 5000.0 },
                    { 24, 10000.0, 4, 9, 5000.0 },
                    { 25, 10000.0, 2, 10, 5000.0 },
                    { 26, 10000.0, 3, 10, 5000.0 },
                    { 27, 10000.0, 4, 10, 5000.0 },
                    { 28, 10000.0, 1, 11, 5000.0 },
                    { 29, 10000.0, 2, 11, 5000.0 },
                    { 30, 10000.0, 3, 11, 5000.0 },
                    { 31, 10000.0, 4, 11, 5000.0 },
                    { 32, 10000.0, 1, 12, 5000.0 },
                    { 33, 10000.0, 2, 12, 5000.0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "GasStationId", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 5, null, "Manager Station 3", 3, "manager", null, "Manager", "manager3" },
                    { 6, null, "Manager Station 4", 4, "manager", null, "Manager", "manager4" },
                    { 7, null, "Manager Station 5", 5, "manager", null, "Manager", "manager5" },
                    { 8, null, "Manager Station 6", 6, "manager", null, "Manager", "manager6" },
                    { 9, null, "Manager Station 7", 7, "manager", null, "Manager", "manager7" },
                    { 10, null, "Manager Station 8", 8, "manager", null, "Manager", "manager8" },
                    { 11, null, "Manager Station 9", 9, "manager", null, "Manager", "manager9" },
                    { 12, null, "Manager Station 10", 10, "manager", null, "Manager", "manager10" },
                    { 13, null, "Manager Station 11", 11, "manager", null, "Manager", "manager11" },
                    { 14, null, "Manager Station 12", 12, "manager", null, "Manager", "manager12" }
                });

            migrationBuilder.InsertData(
                table: "PumpTank",
                columns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                values: new object[,]
                {
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 },
                    { 5, 8 },
                    { 6, 5 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 7, 5 },
                    { 7, 6 },
                    { 7, 7 },
                    { 7, 8 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 9, 10 },
                    { 10, 9 },
                    { 10, 10 },
                    { 11, 9 },
                    { 11, 10 },
                    { 12, 9 },
                    { 12, 10 },
                    { 13, 11 },
                    { 13, 12 },
                    { 13, 13 },
                    { 14, 11 },
                    { 14, 12 },
                    { 14, 13 },
                    { 15, 11 },
                    { 15, 12 },
                    { 15, 13 },
                    { 16, 11 },
                    { 16, 12 },
                    { 16, 13 },
                    { 17, 14 },
                    { 17, 15 },
                    { 17, 16 },
                    { 17, 17 },
                    { 18, 14 },
                    { 18, 15 },
                    { 18, 16 },
                    { 18, 17 },
                    { 19, 14 },
                    { 19, 15 },
                    { 19, 16 },
                    { 19, 17 },
                    { 20, 14 },
                    { 20, 15 },
                    { 20, 16 },
                    { 20, 17 },
                    { 21, 18 },
                    { 21, 19 },
                    { 22, 18 },
                    { 22, 19 },
                    { 23, 18 },
                    { 23, 19 },
                    { 24, 18 },
                    { 24, 19 },
                    { 25, 20 },
                    { 26, 20 },
                    { 27, 20 },
                    { 28, 20 },
                    { 29, 21 },
                    { 29, 22 },
                    { 29, 23 },
                    { 29, 24 },
                    { 30, 21 },
                    { 30, 22 },
                    { 30, 23 },
                    { 30, 24 },
                    { 31, 21 },
                    { 31, 22 },
                    { 31, 23 },
                    { 31, 24 },
                    { 32, 21 },
                    { 32, 22 },
                    { 32, 23 },
                    { 32, 24 },
                    { 33, 25 },
                    { 33, 26 },
                    { 33, 27 },
                    { 34, 25 },
                    { 34, 26 },
                    { 34, 27 },
                    { 35, 25 },
                    { 35, 26 },
                    { 35, 27 },
                    { 36, 25 },
                    { 36, 26 },
                    { 36, 27 },
                    { 37, 28 },
                    { 37, 29 },
                    { 37, 30 },
                    { 37, 31 },
                    { 38, 28 },
                    { 38, 29 },
                    { 38, 30 },
                    { 38, 31 },
                    { 39, 28 },
                    { 39, 29 },
                    { 39, 30 },
                    { 39, 31 },
                    { 40, 28 },
                    { 40, 29 },
                    { 40, 30 },
                    { 40, 31 },
                    { 41, 32 },
                    { 41, 33 },
                    { 42, 32 },
                    { 42, 33 },
                    { 43, 32 },
                    { 43, 33 },
                    { 44, 32 },
                    { 44, 33 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 5, 7 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 5, 8 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 6, 8 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 7, 6 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 7, 8 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 8, 7 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 8, 8 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 9, 9 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 9, 10 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 10, 9 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 10, 10 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 11, 9 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 11, 10 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 12, 9 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 12, 10 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 13, 11 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 13, 12 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 13, 13 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 14, 11 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 14, 12 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 14, 13 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 15, 11 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 15, 12 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 15, 13 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 16, 11 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 16, 12 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 16, 13 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 17, 14 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 17, 15 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 17, 16 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 17, 17 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 18, 14 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 18, 15 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 18, 16 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 18, 17 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 19, 14 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 19, 15 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 19, 16 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 19, 17 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 20, 14 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 20, 15 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 20, 16 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 20, 17 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 21, 18 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 21, 19 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 22, 18 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 22, 19 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 23, 18 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 23, 19 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 24, 18 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 24, 19 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 25, 20 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 26, 20 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 27, 20 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 28, 20 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 29, 21 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 29, 22 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 29, 23 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 29, 24 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 30, 21 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 30, 22 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 30, 23 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 30, 24 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 31, 21 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 31, 22 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 31, 23 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 31, 24 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 32, 21 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 32, 22 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 32, 23 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 32, 24 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 33, 25 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 33, 26 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 33, 27 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 34, 25 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 34, 26 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 34, 27 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 35, 25 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 35, 26 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 35, 27 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 36, 25 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 36, 26 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 36, 27 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 37, 28 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 37, 29 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 37, 30 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 37, 31 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 38, 28 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 38, 29 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 38, 30 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 38, 31 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 39, 28 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 39, 29 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 39, 30 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 39, 31 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 40, 28 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 40, 29 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 40, 30 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 40, 31 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 41, 32 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 41, 33 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 42, 32 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 42, 33 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 43, 32 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 43, 33 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 44, 32 });

            migrationBuilder.DeleteData(
                table: "PumpTank",
                keyColumns: new[] { "ConnectedPumpsId", "ConnectedTanksId" },
                keyValues: new object[] { 44, 33 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Pumps",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}

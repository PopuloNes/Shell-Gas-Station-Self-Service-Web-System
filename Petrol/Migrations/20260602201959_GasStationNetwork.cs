using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class GasStationNetwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GasStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pumps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GasStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pumps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pumps_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GasStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    FuelTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Capacity = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanks_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tanks_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GasStationId = table.Column<int>(type: "INTEGER", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    PumpId = table.Column<int>(type: "INTEGER", nullable: true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BarCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_FuelTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Pumps_PumpId",
                        column: x => x.PumpId,
                        principalTable: "Pumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PumpTank",
                columns: table => new
                {
                    ConnectedPumpsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectedTanksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PumpTank", x => new { x.ConnectedPumpsId, x.ConnectedTanksId });
                    table.ForeignKey(
                        name: "FK_PumpTank_Pumps_ConnectedPumpsId",
                        column: x => x.ConnectedPumpsId,
                        principalTable: "Pumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PumpTank_Tanks_ConnectedTanksId",
                        column: x => x.ConnectedTanksId,
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonusCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ClientName = table.Column<string>(type: "TEXT", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    BonusBalance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BonusCardId = table.Column<int>(type: "INTEGER", nullable: true),
                    BonusSpent = table.Column<double>(type: "REAL", nullable: false),
                    AccruedBonuses = table.Column<double>(type: "REAL", nullable: false),
                    GasStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentStatus = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity = table.Column<double>(type: "REAL", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "BonusCards",
                columns: new[] { "Id", "Barcode", "BonusBalance", "ClientName", "UserId" },
                values: new object[,]
                {
                    { 1, "1234", 100.0, "John Doe", null },
                    { 2, "5678", 200.0, "Jane Smith", null }
                });

            migrationBuilder.InsertData(
                table: "FuelTypes",
                columns: new[] { "Id", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { 1, false, "A-95", 5.0 },
                    { 2, false, "A-92", 4.0 },
                    { 3, false, "Diesel", 6.0 },
                    { 4, false, "Gas", 3.0 }
                });

            migrationBuilder.InsertData(
                table: "GasStations",
                columns: new[] { "Id", "Address", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, "Złota 59, 00-120 Warszawa", 52.229999999999997, 21.002400000000002, "Shell Warszawa Centrum" },
                    { 2, "Targowa 12, 03-736 Warszawa", 52.251399999999997, 21.041599999999999, "Shell Warszawa Praga" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "GasStationId", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, null, "Михайло Стадніков", null, "admin", null, "Admin", "admin" },
                    { 2, null, "Олексій Романенко", null, "cashier", null, "Cashier", "cashier" }
                });

            migrationBuilder.InsertData(
                table: "Pumps",
                columns: new[] { "Id", "GasStationId", "Name", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Pump 1", 2 },
                    { 2, 1, "Pump 2", 2 },
                    { 3, 2, "Pump 3", 2 },
                    { 4, 2, "Pump 4", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tanks",
                columns: new[] { "Id", "Capacity", "FuelTypeId", "GasStationId", "Volume" },
                values: new object[,]
                {
                    { 1, 10000.0, 1, 1, 5000.0 },
                    { 2, 10000.0, 2, 1, 5000.0 },
                    { 3, 10000.0, 3, 2, 5000.0 },
                    { 4, 10000.0, 4, 2, 5000.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusCards_UserId",
                table: "BonusCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PumpId",
                table: "Products",
                column: "PumpId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_GasStationId",
                table: "Pumps",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_PumpTank_ConnectedTanksId",
                table: "PumpTank",
                column: "ConnectedTanksId");

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_FuelTypeId",
                table: "Tanks",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_GasStationId",
                table: "Tanks",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GasStationId",
                table: "Users",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusCards");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "PumpTank");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tanks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Pumps");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "GasStations");
        }
    }
}

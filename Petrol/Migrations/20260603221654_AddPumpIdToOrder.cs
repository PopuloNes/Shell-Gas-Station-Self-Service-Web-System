using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class AddPumpIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PumpId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PumpId",
                table: "Orders",
                column: "PumpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Pumps_PumpId",
                table: "Orders",
                column: "PumpId",
                principalTable: "Pumps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Pumps_PumpId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PumpId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PumpId",
                table: "Orders");
        }
    }
}

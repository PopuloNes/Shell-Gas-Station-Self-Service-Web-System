using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "FullName",
                value: "admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "FullName",
                value: "Михайло Стадніков");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "GasStationId", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[] { 2, null, "Олексій Романенко", null, "cashier", null, "Client", "cashier" });
        }
    }
}

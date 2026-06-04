using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class AddManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "GasStationId", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 3, null, "Manager Station 1", 1, "manager", null, "Manager", "manager1" },
                    { 4, null, "Manager Station 2", 2, "manager", null, "Manager", "manager2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class AdminPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PhoneNumber" },
                values: new object[] { "password", "+48000000001" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "PhoneNumber" },
                values: new object[] { "admin", null });
        }
    }
}

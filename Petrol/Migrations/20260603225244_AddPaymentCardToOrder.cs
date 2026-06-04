using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrol.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentCardToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentCardNumber",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCardNumber",
                table: "Orders");
        }
    }
}

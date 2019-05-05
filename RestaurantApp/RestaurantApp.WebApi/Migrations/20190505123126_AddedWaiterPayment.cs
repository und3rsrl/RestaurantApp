using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.WebApi.Migrations
{
    public partial class AddedWaiterPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WaiterPayment",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaiterPayment",
                table: "Orders");
        }
    }
}

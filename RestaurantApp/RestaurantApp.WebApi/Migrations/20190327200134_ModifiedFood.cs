using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.WebApi.Migrations
{
    public partial class ModifiedFood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Categorie",
                table: "Foods",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Foods",
                newName: "Categorie");
        }
    }
}

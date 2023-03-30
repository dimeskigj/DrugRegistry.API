using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugRegistry.API.Migrations
{
    /// <inheritdoc />
    public partial class RenamePropertyInDrug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceWIthoutVat",
                table: "Drugs",
                newName: "PriceWithoutVat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceWithoutVat",
                table: "Drugs",
                newName: "PriceWIthoutVat");
        }
    }
}

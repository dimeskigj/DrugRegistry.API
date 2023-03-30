using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugRegistry.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlToPharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Pharmacies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Pharmacies");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugRegistry.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLastUpdateToPharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Pharmacies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 29, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Pharmacies");
        }
    }
}

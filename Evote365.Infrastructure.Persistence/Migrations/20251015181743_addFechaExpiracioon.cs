using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFechaExpiracioon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaExpiracion",
                table: "AlianzasPoliticas",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaExpiracion",
                table: "AlianzasPoliticas");
        }
    }
}

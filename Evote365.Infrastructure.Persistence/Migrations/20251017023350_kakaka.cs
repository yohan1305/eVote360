using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class kakaka : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PuestoElectivoId",
                table: "PuestosElectivos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PuestoElectivoId",
                table: "PuestosElectivos");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Kloq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AsignacionesCandidatosPuestos_CandidatoId",
                table: "AsignacionesCandidatosPuestos");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesCandidatosPuestos_CandidatoId_PartidoPoliticoId_PuestoElectivoId",
                table: "AsignacionesCandidatosPuestos",
                columns: new[] { "CandidatoId", "PartidoPoliticoId", "PuestoElectivoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AsignacionesCandidatosPuestos_CandidatoId_PartidoPoliticoId_PuestoElectivoId",
                table: "AsignacionesCandidatosPuestos");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesCandidatosPuestos_CandidatoId",
                table: "AsignacionesCandidatosPuestos",
                column: "CandidatoId");
        }
    }
}

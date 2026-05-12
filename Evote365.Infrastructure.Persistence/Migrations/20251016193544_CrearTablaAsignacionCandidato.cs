using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaAsignacionCandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Ciudadanos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AsignacionesCandidatosPuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidatoId = table.Column<int>(type: "int", nullable: false),
                    PartidoPoliticoId = table.Column<int>(type: "int", nullable: false),
                    PuestoElectivoId = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesCandidatosPuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionesCandidatosPuestos_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionesCandidatosPuestos_PartidosPoliticos_PartidoPoliticoId",
                        column: x => x.PartidoPoliticoId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionesCandidatosPuestos_PuestosElectivos_PuestoElectivoId",
                        column: x => x.PuestoElectivoId,
                        principalTable: "PuestosElectivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesCandidatosPuestos_CandidatoId",
                table: "AsignacionesCandidatosPuestos",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesCandidatosPuestos_PartidoPoliticoId",
                table: "AsignacionesCandidatosPuestos",
                column: "PartidoPoliticoId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesCandidatosPuestos_PuestoElectivoId",
                table: "AsignacionesCandidatosPuestos",
                column: "PuestoElectivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignacionesCandidatosPuestos");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Ciudadanos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

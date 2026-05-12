using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ciudadanos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentoIdentidad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudadanos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elecciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elecciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartidosPoliticos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Siglas = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartidosPoliticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PuestosElectivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestosElectivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlianzasPoliticas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartidoSolicitanteId = table.Column<int>(type: "int", nullable: false),
                    PartidoReceptorId = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlianzasPoliticas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlianzasPoliticas_PartidosPoliticos_PartidoReceptorId",
                        column: x => x.PartidoReceptorId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AlianzasPoliticas_PartidosPoliticos_PartidoSolicitanteId",
                        column: x => x.PartidoSolicitanteId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    PartidoAsignadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_PartidosPoliticos_PartidoAsignadoId",
                        column: x => x.PartidoAsignadoId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EleccionPuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EleccionId = table.Column<int>(type: "int", nullable: false),
                    PuestoElectivoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EleccionPuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EleccionPuestos_Elecciones_EleccionId",
                        column: x => x.EleccionId,
                        principalTable: "Elecciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EleccionPuestos_PuestosElectivos_PuestoElectivoId",
                        column: x => x.PuestoElectivoId,
                        principalTable: "PuestosElectivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    PartidoPoliticoId = table.Column<int>(type: "int", nullable: false),
                    PuestoElectivoId = table.Column<int>(type: "int", nullable: true),
                    EleccionPuestoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatos_EleccionPuestos_EleccionPuestoId",
                        column: x => x.EleccionPuestoId,
                        principalTable: "EleccionPuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidatos_PartidosPoliticos_PartidoPoliticoId",
                        column: x => x.PartidoPoliticoId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatos_PuestosElectivos_PuestoElectivoId",
                        column: x => x.PuestoElectivoId,
                        principalTable: "PuestosElectivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Votos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CiudadanoId = table.Column<int>(type: "int", nullable: false),
                    EleccionId = table.Column<int>(type: "int", nullable: false),
                    PuestoElectivoId = table.Column<int>(type: "int", nullable: false),
                    CandidatoId = table.Column<int>(type: "int", nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votos_Candidatos_CandidatoId",
                        column: x => x.CandidatoId,
                        principalTable: "Candidatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Votos_Ciudadanos_CiudadanoId",
                        column: x => x.CiudadanoId,
                        principalTable: "Ciudadanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votos_Elecciones_EleccionId",
                        column: x => x.EleccionId,
                        principalTable: "Elecciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votos_PuestosElectivos_PuestoElectivoId",
                        column: x => x.PuestoElectivoId,
                        principalTable: "PuestosElectivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlianzasPoliticas_PartidoReceptorId",
                table: "AlianzasPoliticas",
                column: "PartidoReceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_AlianzasPoliticas_PartidoSolicitanteId",
                table: "AlianzasPoliticas",
                column: "PartidoSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_EleccionPuestoId",
                table: "Candidatos",
                column: "EleccionPuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PartidoPoliticoId",
                table: "Candidatos",
                column: "PartidoPoliticoId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_PuestoElectivoId",
                table: "Candidatos",
                column: "PuestoElectivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudadanos_DocumentoIdentidad",
                table: "Ciudadanos",
                column: "DocumentoIdentidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EleccionPuestos_EleccionId",
                table: "EleccionPuestos",
                column: "EleccionId");

            migrationBuilder.CreateIndex(
                name: "IX_EleccionPuestos_PuestoElectivoId",
                table: "EleccionPuestos",
                column: "PuestoElectivoId");

            migrationBuilder.CreateIndex(
                name: "IX_PartidosPoliticos_Siglas",
                table: "PartidosPoliticos",
                column: "Siglas",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PartidoAsignadoId",
                table: "Usuarios",
                column: "PartidoAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CandidatoId",
                table: "Votos",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_CiudadanoId",
                table: "Votos",
                column: "CiudadanoId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_EleccionId",
                table: "Votos",
                column: "EleccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_PuestoElectivoId",
                table: "Votos",
                column: "PuestoElectivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlianzasPoliticas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Votos");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Ciudadanos");

            migrationBuilder.DropTable(
                name: "EleccionPuestos");

            migrationBuilder.DropTable(
                name: "PartidosPoliticos");

            migrationBuilder.DropTable(
                name: "Elecciones");

            migrationBuilder.DropTable(
                name: "PuestosElectivos");
        }
    }
}

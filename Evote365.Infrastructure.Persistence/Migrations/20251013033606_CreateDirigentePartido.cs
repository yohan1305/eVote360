using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evote365.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateDirigentePartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirigentesPartidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PartidoPoliticoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirigentesPartidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirigentesPartidos_PartidosPoliticos_PartidoPoliticoId",
                        column: x => x.PartidoPoliticoId,
                        principalTable: "PartidosPoliticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirigentesPartidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirigentesPartidos_PartidoPoliticoId",
                table: "DirigentesPartidos",
                column: "PartidoPoliticoId");

            migrationBuilder.CreateIndex(
                name: "IX_DirigentesPartidos_UsuarioId",
                table: "DirigentesPartidos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirigentesPartidos");
        }
    }
}

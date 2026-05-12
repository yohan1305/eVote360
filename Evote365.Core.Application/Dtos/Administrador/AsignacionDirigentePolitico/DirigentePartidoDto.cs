using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico
{
    public class DirigentePartidoDto
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public int PartidoPoliticoId { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
    }
}

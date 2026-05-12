using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos
{
    public class AsignacionListadoDto
    {
        public int CandidatoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public string NombrePuesto { get; set; } = string.Empty;
        public DateTime FechaAsignacion { get; set; }
        public bool EsAliado { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos
{
    public class CandidatoDisponibleDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string PartidoSiglas { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public bool YaTienePuesto { get; set; }
        public string? NombrePuestoActual { get; set; }
        public bool EsAliado { get; set; }
    }

}

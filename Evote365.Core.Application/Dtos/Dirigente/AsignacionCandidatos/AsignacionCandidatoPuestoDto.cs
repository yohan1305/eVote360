using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos
{
    public class AsignacionCandidatoPuestoDto
    {
        public int CandidatoId { get; set; }
        public int PuestoElectivoId { get; set; }
        public int PartidoPoliticoId { get; set; } // quien asigna
        public string? NombreCompleto { get; set; }
        public string? PartidoSiglas { get; set; }
        public string? NombrePuesto { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }

}

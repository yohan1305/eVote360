using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.Candidato
{
    public class CandidatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? FotoUrl { get; set; }
        public EstadoEntidad Estado { get; set; }

        public string PartidoSiglas { get; set; } = null!;
        public string? NombrePuesto { get; set; } // "Sin puesto asociado" si null

        public List<string> PuestosAsignados { get; set; } = new();


    }
}

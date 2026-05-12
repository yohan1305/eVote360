using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class Candidato
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? FotoUrl { get; set; }

        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;

        public required int PartidoPoliticoId { get; set; }
        public required PartidoPolitico PartidoPolitico { get; set; }

        public int? PuestoElectivoId { get; set; }
        public PuestoElectivo? PuestoElectivo { get; set; }

        public ICollection<Voto> VotosRecibidos { get; set; } = new List<Voto>();

        public ICollection<AsignacionCandidatoPuesto> Asignaciones { get; set; } = new List<AsignacionCandidatoPuesto>();
    }
}

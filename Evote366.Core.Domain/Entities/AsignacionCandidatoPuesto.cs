using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class AsignacionCandidatoPuesto
    {
        public int Id { get; set; }

        // Candidato asignado
        public int CandidatoId { get; set; }
        public Candidato Candidato { get; set; }

        // Partido que lo está asignando (puede ser aliado)
        public int PartidoPoliticoId { get; set; }
        public PartidoPolitico PartidoPolitico { get; set; }

        // Puesto al que se le asigna dentro de ese partido
        public int PuestoElectivoId { get; set; }
        public PuestoElectivo PuestoElectivo { get; set; }

        // Fecha de asignación
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class EleccionPuesto
    {
        public int Id { get; set; }

        public required int EleccionId { get; set; }
        public Eleccion? Eleccion { get; set; }

        public required int PuestoElectivoId { get; set; }
        public PuestoElectivo? PuestoElectivo { get; set; }

        public ICollection<Candidato> CandidatosAsignados { get; set; } = new List<Candidato>();
    }
}

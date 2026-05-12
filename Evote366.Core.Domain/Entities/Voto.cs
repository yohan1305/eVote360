using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class Voto
    {
        public int Id { get; set; }

        public required int CiudadanoId { get; set; }
        public required Ciudadano Ciudadano { get; set; }

        public required int EleccionId { get; set; }
        public required Eleccion Eleccion { get; set; }

        public required int PuestoElectivoId { get; set; }
        public required PuestoElectivo PuestoElectivo { get; set; }

        public int PartidoPoliticoId { get; set; }

        public int? CandidatoId { get; set; }
        public Candidato? Candidato { get; set; }

        public required DateTime FechaEmision { get; set; }
    }
}

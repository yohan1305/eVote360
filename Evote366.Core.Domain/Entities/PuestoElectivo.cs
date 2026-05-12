using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class PuestoElectivo
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }

        public EstadoEntidad Estado { get; set; }

        public ICollection<AsignacionCandidatoPuesto> Asignaciones { get; set; } = new List<AsignacionCandidatoPuesto>();

        // Navegación
        public ICollection<Candidato> Candidatos { get; set; } = new List<Candidato>();
        public ICollection<EleccionPuesto> EleccionesAsociadas { get; set; } = new List<EleccionPuesto>();
    }

}

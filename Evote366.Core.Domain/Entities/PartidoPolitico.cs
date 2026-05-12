using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class PartidoPolitico
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required string Siglas { get; set; }
        public required string LogoUrl { get; set; }

        public EstadoEntidad Estado { get; set; }

        // Navegación
        public ICollection<Usuario> Dirigentes { get; set; } = new List<Usuario>();
        public ICollection<Candidato> Candidatos { get; set; } = new List<Candidato>();
        public ICollection<AlianzaPolitica> AlianzasEnviadas { get; set; } = new List<AlianzaPolitica>();
        public ICollection<AlianzaPolitica> AlianzasRecibidas { get; set; } = new List<AlianzaPolitica>();

        public ICollection<AsignacionCandidatoPuesto> Asignaciones { get; set; } = new List<AsignacionCandidatoPuesto>();
    }

}

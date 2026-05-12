using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.Candidato
{
    public class ConfirmarCambioEstadoCandidatoDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public bool Activar { get; set; } // true = activar, false = desactivar
    }
}

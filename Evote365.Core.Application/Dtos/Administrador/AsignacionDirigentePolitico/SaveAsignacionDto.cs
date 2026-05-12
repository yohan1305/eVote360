using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico
{
    public class SaveAsignacionDto
    {
        public int UsuarioId { get; set; }
        public int PartidoPoliticoId { get; set; }
    }
}

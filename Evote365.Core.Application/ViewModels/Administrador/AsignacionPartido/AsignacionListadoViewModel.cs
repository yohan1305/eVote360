using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido
{
    public class AsignacionListadoViewModel
    {
        public List<DirigentePartidoDto> Relaciones { get; set; } = new();
        public bool PuedeModificar { get; set; } = true;
    }
}

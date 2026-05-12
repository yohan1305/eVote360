using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.PartidoPolitico
{
    public class ToggleEstadoPartidoViewModel
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required EstadoEntidad EstadoActual { get; set; }
    }
}

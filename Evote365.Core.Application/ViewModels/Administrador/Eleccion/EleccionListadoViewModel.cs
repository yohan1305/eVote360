using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class EleccionListadoViewModel
    {
        public List<EleccionDto> Elecciones { get; set; } = new();
        public bool HayEleccionActiva { get; set; }
        public int? EleccionActivaId { get; set; }
    }
}

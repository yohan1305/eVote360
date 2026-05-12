using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class AsignacionPuestosViewModel
    {
        public int EleccionId { get; set; }

        public List<PuestoAsignableViewModel> PuestosDisponibles { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class PuestoAsignableViewModel
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public bool Seleccionado { get; set; } // ← checkbox en la vista
    }
}

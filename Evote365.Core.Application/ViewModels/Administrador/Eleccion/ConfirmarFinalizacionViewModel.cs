using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class ConfirmarFinalizacionViewModel
    {
        public int EleccionId { get; set; }
        public string NombreEleccion { get; set; } = null!;
        public DateTime FechaRealizacion { get; set; }
    }
}

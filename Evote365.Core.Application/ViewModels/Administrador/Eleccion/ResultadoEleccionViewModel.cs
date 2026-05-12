using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class ResultadoEleccionViewModel
    {
        public string NombreEleccion { get; set; } = null!;
        public DateTime FechaRealizacion { get; set; }
        public List<ResultadoPuestoViewModel> ResultadosPorPuesto { get; set; } = new();
    }
}

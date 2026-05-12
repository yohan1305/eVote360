using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{
    public class ResultadoEleccionDto
    {
        public string NombreEleccion { get; set; } = null!;
        public DateTime FechaRealizacion { get; set; }
        public List<ResultadoPuestoDto> ResultadosPorPuesto { get; set; } = new();
    }
}

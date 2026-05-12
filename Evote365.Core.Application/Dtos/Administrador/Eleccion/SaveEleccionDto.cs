using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{
    public class SaveEleccionDto
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaRealizacion { get; set; }
    }
}

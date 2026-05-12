using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{
    public class PuestoAsignableDto
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public bool EstaAsignado { get; set; }
    }
}

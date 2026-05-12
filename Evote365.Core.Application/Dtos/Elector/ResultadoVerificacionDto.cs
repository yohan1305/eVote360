using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class ResultadoVerificacionDto
    {
        public bool EleccionActiva { get; set; }
        public bool CiudadanoExiste { get; set; }
        public bool CiudadanoActivo { get; set; }
        public bool YaVoto { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}

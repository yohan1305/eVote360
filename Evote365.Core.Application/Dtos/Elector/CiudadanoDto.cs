using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class CiudadanoDto
    {
        public int Id { get; set; }
        public required string DocumentoIdentidad { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Email { get; set; }
        public EstadoEntidad Estado { get; set; }
    }
}

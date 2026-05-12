using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Ciudadano
{
    public class CiudadanoDto
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string DocumentoIdentidad { get; set; }

        public bool YaVoto { get; set; } = false;
        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;
    }

}

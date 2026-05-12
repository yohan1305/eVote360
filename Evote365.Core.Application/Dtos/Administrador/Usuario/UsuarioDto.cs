using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Usuario
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string NombreUsuario { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public RolUsuario Rol { get; set; }
        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;

        public int? PartidoAsignadoId { get; set; }

    }

}

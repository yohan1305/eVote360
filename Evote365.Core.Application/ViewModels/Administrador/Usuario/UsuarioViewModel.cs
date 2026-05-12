using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Usuario
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;

        public RolUsuario Rol { get; set; }
        public EstadoEntidad Estado { get; set; }
    }
}

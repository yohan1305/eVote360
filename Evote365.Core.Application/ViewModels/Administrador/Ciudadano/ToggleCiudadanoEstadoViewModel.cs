using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Ciudadano
{
    public class ToggleCiudadanoEstadoViewModel
    {
        public int Id { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public EstadoEntidad Estado { get; set; }
    }
}

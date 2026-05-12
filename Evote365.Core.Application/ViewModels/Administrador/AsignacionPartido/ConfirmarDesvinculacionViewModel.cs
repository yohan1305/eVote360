using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido
{
    public class ConfirmarDesvinculacionViewModel
    {
        public int RelacionId { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class ValidarIdentidadViewModel
    {
        public string Cedula { get; set; } = string.Empty;

        public string? MensajeOCR { get; set; }

        public bool PermitidoProcesar { get; set; } = true;
    }
}

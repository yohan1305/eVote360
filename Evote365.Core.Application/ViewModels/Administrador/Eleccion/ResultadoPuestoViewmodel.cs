using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class ResultadoPuestoViewModel
    {
        public string NombrePuesto { get; set; } = null!;
        public List<ResultadoCandidatoViewModel> Candidatos { get; set; } = new();
    }
}

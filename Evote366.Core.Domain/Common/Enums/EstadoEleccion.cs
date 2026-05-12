using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Common.Enums
{
    public enum EstadoEleccion
    {
        EnProceso = 1,     // Elección activa, en curso
        Finalizada = 2,    // Elección cerrada, resultados disponibles
        Cancelada = 3      // (Opcional) Elección anulada o descartada
    }
}

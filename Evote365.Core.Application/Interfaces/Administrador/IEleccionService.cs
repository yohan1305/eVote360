using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IEleccionService : IGenericService<EleccionDto>
    {
        // Crear elección con validaciones críticas
        Task<(bool Exito, List<string> Mensajes)> CrearEleccionAsync(SaveEleccionDto dto);

        // Finalizar elección activa
        Task<bool> FinalizarEleccionAsync(int eleccionId);

        // Obtener resultados por elección
        Task<ResultadoEleccionDto?> GetResultadosPorEleccion(int eleccionId);

        // Verificar si hay elección activa
        Task<bool> HayEleccionActiva();

        // Obtener elección activa
        Task<EleccionDto?> GetEleccionActiva();

        Task<Eleccion?> GetEleccionForActiva();

        // Ya existentes
        Task<EleccionDto?> GetEleccionActivaAsync();
        Task<EleccionDto?> GetEleccionActivaForElectorAsync();

        // 🔹 NUEVOS métodos para asignación de puestos
        Task<List<PuestoAsignableDto>> ObtenerPuestosAsignablesAsync(int eleccionId);
        Task AsignarPuestosAEleccionAsync(AsignacionPuestosDto dto);
    }
}

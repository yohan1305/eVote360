using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IEleccionRepository : IGenericRepository<Eleccion>
    {
        Task<Eleccion?> GetEleccionActiva();

        Task<Eleccion?> GetEleccionActivaForElector();

        Task<List<int>> GetPuestosAsignadosAsync(int eleccionId);

        Task AsignarPuestosAEleccionAsync(int eleccionId, List<int> puestosIds);
    }
}

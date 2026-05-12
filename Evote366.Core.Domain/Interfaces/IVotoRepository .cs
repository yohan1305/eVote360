using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IVotoRepository : IGenericRepository<Voto>
    {
        Task<bool> ExisteVotoAsync(int ciudadanoId, int eleccionId, int puestoElectivoId);
        Task<Voto?> RegistrarVotoAsync(Voto voto);
        Task<List<Voto>> GetResumenVotosAsync(int ciudadanoId, int eleccionId);
    }
}

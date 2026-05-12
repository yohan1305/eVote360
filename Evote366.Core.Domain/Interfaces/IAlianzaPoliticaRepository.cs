using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IAlianzaPoliticaRepository : IGenericRepository<AlianzaPolitica>
    {
        Task<List<AlianzaPolitica>> GetSolicitudesRecibidasAsync(int partidoId);
        Task<List<AlianzaPolitica>> GetSolicitudesEnviadasAsync(int partidoId);
        Task<List<AlianzaPolitica>> GetAlianzasVigentesAsync(int partidoId);
        Task<bool> ExisteRelacionPendienteAsync(int partidoAId, int partidoBId);
        Task<bool> ExisteAlianzaActivaAsync(int partidoAId, int partidoBId);

        Task<bool> ExisteAlianzaEntre(int partidoAId, int partidoBId);

        Task<List<int>> GetAliadosVigentesIdsAsync(int partidoId);


    }

}

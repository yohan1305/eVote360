using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface ICiudadanoRepository : IGenericRepository<Ciudadano>
    {
        Task<Ciudadano?> GetByCedulaAsync(string cedula);

        Task<Ciudadano?> GetByDocumentoIdentidadAsync(string documento);
        Task<bool> ExistsByDocumentoIdentidadAsync(string documento);

        Task<bool> YaHaVotadoAsync(int ciudadanoId, int eleccionId);
        Task<List<int>> GetPuestosVotadosAsync(int ciudadanoId, int eleccionId);
        Task MarcarVotacionFinalizadaAsync(int ciudadanoId);



    }
}

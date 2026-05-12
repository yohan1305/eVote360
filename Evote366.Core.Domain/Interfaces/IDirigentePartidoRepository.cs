using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IDirigentePartidoRepository : IGenericRepository<DirigentePartido>
    {
        Task<bool> DirigenteYaAsignado(int usuarioId);
        Task<List<DirigentePartido>> GetAllWithIncludes();

        Task<List<int>> GetIdsDirigentesAsignados();

    }
}

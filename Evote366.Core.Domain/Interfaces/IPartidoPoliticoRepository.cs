using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IPartidoPoliticoRepository : IGenericRepository<PartidoPolitico>
    {
        Task<List<PartidoPolitico>> GetActivos();
    }  
}

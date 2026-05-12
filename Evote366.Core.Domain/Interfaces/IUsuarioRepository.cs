using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task AsignarPartido(int usuarioId, int partidoId);
    }  
}

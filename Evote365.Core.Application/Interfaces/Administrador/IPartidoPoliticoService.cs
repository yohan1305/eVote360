using Evote365.Core.Application.Dtos.Administrador.PartidoPoliticoDto;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IPartidoPoliticoService : IGenericService<PartidoPoliticoDto>
    {
        Task<bool> CanModify();
        Task<bool> SiglasExistente(string siglas, int? id = null);
        Task ToggleEstadoAsync(int id);

        Task<PartidoPoliticoDto?> AddAsync(PartidoPoliticoDto dto);

        Task<List<PartidoPoliticoDto>> GetPartidosActivos();
    }
}

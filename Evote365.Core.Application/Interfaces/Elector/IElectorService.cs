using Evote365.Core.Application.Dtos.Elector;
using CiudadanoDto = Evote365.Core.Application.Dtos.Administrador.Ciudadano.CiudadanoDto;

namespace Evote365.Core.Application.Interfaces.Elector
{
    public interface IElectorService : IGenericService<CiudadanoDto>
    {
        Task<CiudadanoDto?> ObtenerPorDocumentoAsync(string documento);
        Task<bool> EstaActivoAsync(string documento);
        Task<bool> ExistePorDocumentoAsync(string documento);
        Task<bool> YaHaVotadoAsync(string documento, int eleccionId);
        Task<List<int>> GetPuestosVotadosAsync(string documento, int eleccionId);
        Task<bool> RegistrarVotoAsync(string documento, int eleccionId, int puestoId, int candidatoId, int partidoId);
        Task<List<VotoResumenItemDto>> ObtenerResumenVotosAsync(string documento, int eleccionId);
        Task FinalizarVotacionAsync(string documento, int eleccionId);
    }
}

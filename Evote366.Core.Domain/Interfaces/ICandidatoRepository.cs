using Evote366.Core.Domain.Entities;

namespace Evote366.Core.Domain.Interfaces
{
    public interface ICandidatoRepository : IGenericRepository<Candidato>
    {
        Task<List<Candidato>> GetByPartidoIdWithInclude(int partidoId);
        Task<bool> ExisteCandidatoEnPuesto(int partidoId, int puestoId);

        Task<bool> CandidatoYaAsignadoEnPartido(int candidatoId, int partidoId);

        Task<bool> CandidatoAliadoAspiraAlMismoPuesto(int candidatoId, int puestoId);

        Task<List<Candidato>> GetDisponiblesPorPartido(int partidoId);

        Task<List<Candidato>> GetAliadosSinPuesto(List<int> aliadosIds);

        Task<List<Candidato>> GetAliadosConPuesto(int puestoId, List<int> aliadosIds);

        Task<List<Candidato>> GetAsignacionesConPuestoPorPartido(int partidoId);

        Task<List<Candidato>> GetPorPuesto(int puestoId);

        Task<Candidato?> GetByIdWithInclude(int id);

        Task<List<Candidato>> GetDisponiblesPorPartidos(List<int> partidoIds);

        Task<Candidato?> GetByIdAsync(int id);

        Task<List<Candidato>> GetPorPartidosConInclude(List<int> partidoIds);


        Task<List<Candidato>> GetCandidatosDisponiblesParaAsignacion(int partidoId, List<int> aliadosIds, int puestoElectivoId);

        Task<bool> UpdatePuestoElectivoAsync(int candidatoId, int nuevoPuestoId);

        Task<List<Candidato>> GetCandidatosPorPartidoYAliadosAsync(List<int> partidoYAliadosIds);

    }
}

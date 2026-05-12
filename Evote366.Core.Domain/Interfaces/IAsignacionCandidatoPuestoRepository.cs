using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IAsignacionCandidatoPuestoRepository : IGenericRepository<AsignacionCandidatoPuesto>
    {
        Task<bool> ExisteAsignacionAsync(int candidatoId, int partidoId, int puestoId);
        Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorCandidatoAsync(int candidatoId);
        Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorPartidoAsync(int partidoId);
        Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorPuestoAsync(int puestoId);

        Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorCandidatosAsync(List<int> candidatosIds);

    }
}

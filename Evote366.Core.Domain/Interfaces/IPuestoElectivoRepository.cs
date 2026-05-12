using Evote366.Core.Domain.Entities;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Interfaces
{
    public interface IPuestoElectivoRepository : IGenericRepository<PuestoElectivo>
    {
        Task<List<PuestoElectivo>> GetDisponiblesParaPartido(int partidoId);
        Task<PuestoElectivo?> GetByIdAsync(int id);

        Task<bool> ExistePuestoActivoAsync(int puestoId);


        Task<List<PuestoElectivo>> GetActivosConAsignacionesAsync();


        Task<List<PuestoElectivo>> GetPuestosActivosAsync();

    }
}

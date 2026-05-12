using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Dirigente
{
    public interface IAsignacionCandidatoPuestoService : IGenericService<AsignacionCandidatoPuestoDto>
    {

        Task<List<CandidatoDisponibleDto>> GetCandidatosDisponiblesAsync(int puestoId);


        Task<List<PuestoDisponibleDto>> GetPuestosDisponiblesAsync();


        Task<ResultadoOperacionDto> AsignarAsync(AsignacionCandidatoPuestoDto dto);


        Task<ResultadoOperacionDto> DesvincularAsync(int candidatoId);

        Task<List<CandidatoDisponibleDto>> GetRelacionesExistentesAsync(int partidoId);
        Task<CandidatoDisponibleDto?> GetRelacionPorCandidatoAsync(int candidatoId);


        //ultimos
        Task<List<AsignacionCandidatoPuestoDto>> GetAsignacionesPorPuestoAsync(int puestoId);

        Task<List<AsignacionCandidatoPuestoDto>> GetAsignacionesPorPartidoAsync(int partidoId);


    }

}

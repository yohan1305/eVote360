using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Dirigente
{
    public interface ICandidatoService : IGenericService<CandidatoDto>
    {

        Task<List<CandidatoDto>> GetByPartidoDirigente();


        Task<bool> CrearAsync(SaveCandidatoDto dto);


        Task<SaveCandidatoDto?> GetSaveDtoById(int id);


        Task<bool> EditarAsync(SaveCandidatoDto dto);


        Task<ConfirmarCambioEstadoCandidatoDto?> GetConfirmacionCambioEstado(int id, bool activar);


        Task<bool> CambiarEstadoAsync(int id, bool activar);


        Task<bool> CanModify();


        Task<bool> ExisteCandidatoEnPuesto(int puestoId);


    }
}

using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IDirigentePartidoService : IGenericService<DirigentePartidoDto>
    {
        Task<bool> CanModify(); // bloqueo por elección activa
        Task<bool> DirigenteYaAsignado(int usuarioId); //  validación crítica
        Task<List<DirigentePartidoDto>> GetAllWithIncludes(); //  listado con nombres y siglas

        Task<List<int>> GetIdsDirigentesAsignados();

        Task AsignarDirigenteAPartido(int usuarioId, int partidoId);

        Task<bool> DesvincularAsync(int relacionId);


    }
}

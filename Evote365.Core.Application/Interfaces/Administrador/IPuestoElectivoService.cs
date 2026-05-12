using Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IPuestoElectivoService : IGenericService<PuestoElectivoDto>
    {
        Task<List<PuestoElectivoDto>> GetAllWithInclude();
        Task<bool> ToggleEstadoAsync(int id);
        Task<bool> CanModify();

        Task<List<SelectListItem>> GetDisponiblesParaDirigente(int? numero = null);

    }
}

using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface ICiudadanoService : IGenericService<CiudadanoDto>
    {
        Task<bool> CanModify(); //Bloqueo por elección activa
        Task<bool> DocumentoIdentidadExistente(string documento, int? id = null); // Validación de unicidad

        Task<bool> CambiarEstado(int id, EstadoEntidad nuevoEstado);

        Task<bool> PuedeVotar(string cedula);
    }
}

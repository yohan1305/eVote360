using Evote365.Core.Application.Dtos.Administrador.Usuario;
using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces.Administrador
{
    public interface IUsuarioService : IGenericService<UsuarioDto>
    {
        Task<bool> CanModify();
        Task<bool> NombreUsuarioExistente(string nombreUsuario, int? id = null);
        Task<bool> CambiarEstado(int id, EstadoEntidad nuevoEstado);
        Task<UsuarioDto?> AddAsync(UsuarioDto dto, string? plainPassword); // overload con contraseña

        Task<List<UsuarioDto>> GetUsuariosActivosSinAsignacion();

        Task<UsuarioDto?> LoginAsync(string usuario, string contrasena);

        Task<UsuarioDto?> GetByIdAsync(int id);


    }
}

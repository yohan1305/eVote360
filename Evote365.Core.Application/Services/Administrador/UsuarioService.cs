using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Usuario;
using Evote365.Core.Application.Helpers;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Evote365.Core.Application.Services.Administrador
{
    public class UsuarioService : GenericService<Usuario, UsuarioDto>, IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<Eleccion> _eleccionRepository;
        private readonly IMapper _mapper;
        private readonly IDirigentePartidoService _dirigentePartidoService;
        public UsuarioService(
            IGenericRepository<Usuario> usuarioRepository,
            IGenericRepository<Eleccion> eleccionRepository,
            IMapper mapper,
            IDirigentePartidoService dirigentePartidoService

        ) : base(usuarioRepository, mapper)
        {
            _usuarioRepository = usuarioRepository;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
            _dirigentePartidoService = dirigentePartidoService;

        }

        public async Task<bool> CanModify()
        {
            var query = _eleccionRepository.GetAllQuery();
            return !await query.AnyAsync(e => e.Estado == EstadoEleccion.EnProceso);
        }

        public async Task<bool> NombreUsuarioExistente(string nombreUsuario, int? id = null)
        {
            var query = _usuarioRepository.GetAllQuery();

            if (id.HasValue)
                query = query.Where(u => u.Id != id.Value);

            return await query.AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }
        public async Task<List<UsuarioDto>> GetUsuariosActivosSinAsignacion()
        {
            var asignados = await _dirigentePartidoService.GetIdsDirigentesAsignados();

            var query = _usuarioRepository.GetAllQuery()
                .Where(u =>
                    u.Estado == EstadoEntidad.Activo &&
                    u.Rol == RolUsuario.DirigentePolitico &&
                    !asignados.Contains(u.Id));

            var disponibles = await query.ToListAsync();
            return _mapper.Map<List<UsuarioDto>>(disponibles);
        }
        public async Task<bool> CambiarEstado(int id, EstadoEntidad nuevoEstado)
        {
            if (!await CanModify()) return false;

            var actual = await _usuarioRepository.GetById(id);
            if (actual == null) return false;

            actual.Estado = nuevoEstado;
            var result = await _usuarioRepository.UpdateAsync(id, actual);
            return result != null;
        }

        public async Task<UsuarioDto?> AddAsync(UsuarioDto dto, string? plainPassword)
        {
            if (!await CanModify()) return null;

            var entity = _mapper.Map<Usuario>(dto);
            entity.Estado = EstadoEntidad.Activo;

            if (!string.IsNullOrWhiteSpace(plainPassword))
                entity.PasswordHash = PasswordEncryption.Hash(plainPassword);

            var saved = await _usuarioRepository.AddAsync(entity);
            return saved == null ? null : _mapper.Map<UsuarioDto>(saved);
        }

        public override async Task<UsuarioDto?> UpdateAsync(UsuarioDto dto, int id)
        {
            // Verifica si el usuario tiene permiso para modificar
            if (!await CanModify()) return null;

            // Recupera la entidad original desde la base de datos
            var actual = await _usuarioRepository.GetById(id);
            if (actual == null) return null;

            // Mapea el DTO recibido a la entidad
            var entity = _mapper.Map<Usuario>(dto);

            // Preserva el estado original (no editable desde la vista)
            entity.Estado = actual.Estado;

            // Si el hash recibido está vacío, preserva el original; si no, actualiza
            entity.PasswordHash = string.IsNullOrWhiteSpace(dto.PasswordHash)
                ? actual.PasswordHash
                : dto.PasswordHash;

            // Ejecuta la actualización en el repositorio
            var updated = await _usuarioRepository.UpdateAsync(id, entity);

            // Retorna el DTO actualizado o null si falló
            return updated == null ? null : _mapper.Map<UsuarioDto>(updated);
        }

        public async Task<UsuarioDto?> LoginAsync(string usuario, string contrasena)
        {
            var entidad = await _usuarioRepository
          .GetAllQuery()
          .FirstOrDefaultAsync(u => u.NombreUsuario == usuario);

            if (entidad == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(contrasena, entidad.PasswordHash))
                return null;

            return new UsuarioDto
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Apellido = entidad.Apellido,
                Email = entidad.Email,
                NombreUsuario = entidad.NombreUsuario,
                PasswordHash = entidad.PasswordHash,
                Rol = entidad.Rol,
                Estado = entidad.Estado,
                PartidoAsignadoId = entidad.PartidoAsignadoId // aquí lo pasas
            };
        }
        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var entidad = await _usuarioRepository.GetById(id);
            if (entidad == null)
                return null;

            return new UsuarioDto
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Apellido = entidad.Apellido,
                Email = entidad.Email,
                NombreUsuario = entidad.NombreUsuario,
                PasswordHash = entidad.PasswordHash,
                Rol = entidad.Rol,
                Estado = entidad.Estado,
                PartidoAsignadoId = entidad.PartidoAsignadoId
            };
        }

    }
}

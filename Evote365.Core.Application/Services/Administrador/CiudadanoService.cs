using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Evote365.Core.Application.Services.Administrador
{
    public class CiudadanoService : GenericService<Ciudadano, CiudadanoDto>, ICiudadanoService
    {
        private readonly IGenericRepository<Ciudadano> _ciudadanoRepository;
        private readonly IGenericRepository<Eleccion> _eleccionRepository;
        private readonly IMapper _mapper;
        private readonly ICiudadanoRepository _ciudadanoRepository1;


        public CiudadanoService(
            IGenericRepository<Ciudadano> ciudadanoRepository,
            IGenericRepository<Eleccion> eleccionRepository,
            IMapper mapper
,
            ICiudadanoRepository ciudadanoRepository1

        ) : base(ciudadanoRepository, mapper)
        {
            _ciudadanoRepository = ciudadanoRepository;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
            _ciudadanoRepository1 = ciudadanoRepository1;
        }

        // Regla de bloqueo absoluto
        public async Task<bool> CanModify()
        {
            var query = _eleccionRepository.GetAllQuery();
            return !await query.AnyAsync(e => e.Estado == EstadoEleccion.EnProceso);
        }

        // Validación de cédula única
        public async Task<bool> DocumentoIdentidadExistente(string documento, int? id = null)
        {
            var query = _ciudadanoRepository.GetAllQuery();

            if (id.HasValue)
                query = query.Where(c => c.Id != id.Value);

            return await query.AnyAsync(c => c.DocumentoIdentidad == documento);
        }

        //  Crear ciudadano en estado activo
        public override async Task<CiudadanoDto?> AddAsync(CiudadanoDto dto)
        {
            if (!await CanModify()) return null;

            var entity = _mapper.Map<Ciudadano>(dto);

            entity.Estado = EstadoEntidad.Activo; // el sistema controla el estado
            entity.YaVoto = false;                // el sistema garantiza que aún no ha votado

            var saved = await _ciudadanoRepository.AddAsync(entity);
            return saved == null ? null : _mapper.Map<CiudadanoDto>(saved);
        }

        //  Editar preservando estado
        public override async Task<CiudadanoDto?> UpdateAsync(CiudadanoDto dto, int id)
        {
            if (!await CanModify()) return null;

            var actual = await _ciudadanoRepository.GetById(id);
            if (actual == null) return null;

            var entity = _mapper.Map<Ciudadano>(dto);
            entity.Estado = actual.Estado;

            var updated = await _ciudadanoRepository.UpdateAsync(id, entity);
            return updated == null ? null : _mapper.Map<CiudadanoDto>(updated);
        }

        public async Task<bool> PuedeVotar(string cedula)
        {
            var ciudadano = await _ciudadanoRepository1.GetByCedulaAsync(cedula);
            if (ciudadano == null) return false;

            return ciudadano.Estado == EstadoEntidad.Activo && ciudadano.YaVoto == false;
        }
        public async Task<bool> CambiarEstado(int id, EstadoEntidad nuevoEstado)
        {
            if (!await CanModify()) return false;

            var actual = await _ciudadanoRepository.GetById(id);
            if (actual == null) return false;

            actual.Estado = nuevoEstado;
            var result = await _ciudadanoRepository.UpdateAsync(id, actual);
            return result != null;
        }
    }
}

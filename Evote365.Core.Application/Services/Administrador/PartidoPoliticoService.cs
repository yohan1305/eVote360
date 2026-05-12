using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PartidoPoliticoDto;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Core.Application.Services.Administrador
{
    public class PartidoPoliticoService : GenericService<PartidoPolitico, PartidoPoliticoDto>, IPartidoPoliticoService
    {
        private readonly IPartidoPoliticoRepository _partidoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IMapper _mapper;

        public PartidoPoliticoService(
            IPartidoPoliticoRepository partidoRepository,
            IEleccionRepository eleccionRepository,
            IMapper mapper)
            : base(partidoRepository, mapper)
        {
            _partidoRepository = partidoRepository;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
        }

        public async Task<bool> CanModify()
        {
            var eleccionActiva = await _eleccionRepository.GetEleccionActiva();
            return eleccionActiva == null;
        }
        public override async Task<PartidoPoliticoDto?> AddAsync(PartidoPoliticoDto dto)
        {
            try
            {
                var entity = _mapper.Map<PartidoPolitico>(dto);
                entity.Estado = EstadoEntidad.Activo;

                var returnEntity = await _partidoRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return _mapper.Map<PartidoPoliticoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<PartidoPoliticoDto?> UpdateAsync(PartidoPoliticoDto dto, int id)
        {
            try
            {
                var actual = await _partidoRepository.GetById(id);
                if (actual == null) return null;

                var entity = _mapper.Map<PartidoPolitico>(dto);

                // Preservar el estado actual
                entity.Estado = actual.Estado;

                var updated = await _partidoRepository.UpdateAsync(id, entity);
                if (updated == null) return null;

                return _mapper.Map<PartidoPoliticoDto>(updated);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SiglasExistente(string siglas, int? id = null)
        {
            var partidos = await _partidoRepository.GetAllList();
            return partidos.Any(p =>
                p.Siglas.ToLower() == siglas.ToLower() &&
                (!id.HasValue || p.Id != id.Value));
        }

        public async Task ToggleEstadoAsync(int id)
        {
            var partido = await _partidoRepository.GetById(id);
            if (partido == null) return;

            partido.Estado = partido.Estado == EstadoEntidad.Activo
                ? EstadoEntidad.Inactivo
                : EstadoEntidad.Activo;

            await _partidoRepository.UpdateAsync(id, partido);
        }

        public async Task<List<PartidoPoliticoDto>> GetPartidosActivos()
        {
            var query = _partidoRepository.GetAllQuery()
                .Where(p => p.Estado == EstadoEntidad.Activo);

            var activos = await query.ToListAsync();
            return _mapper.Map<List<PartidoPoliticoDto>>(activos);
        }
    }
}

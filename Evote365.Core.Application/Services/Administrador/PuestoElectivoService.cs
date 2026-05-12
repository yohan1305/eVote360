using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Core.Application.Services.Administrador
{
    public class PuestoElectivoService : GenericService<PuestoElectivo, PuestoElectivoDto>, IPuestoElectivoService
    {
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IMapper _mapper;
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IUserSession _userSession;

        public PuestoElectivoService(IPuestoElectivoRepository puestoElectivoRepository,ICandidatoRepository candidatoRepository ,IEleccionRepository eleccionRepository, IMapper mapper, IUserSession userSession)
            : base(puestoElectivoRepository, mapper)
        {
            _puestoElectivoRepository = puestoElectivoRepository;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
            _candidatoRepository = candidatoRepository;
            _userSession = userSession;

        }

        public async Task<List<PuestoElectivoDto>> GetAllWithInclude()
        {
            var query = _puestoElectivoRepository.GetAllQueryWithInclude(["Candidatos", "EleccionesAsociadas"]);
            var list = await query.ProjectTo<PuestoElectivoDto>(_mapper.ConfigurationProvider).ToListAsync();
            return list;
        }

        public async Task<bool> ToggleEstadoAsync(int id)
        {
            var entity = await _puestoElectivoRepository.GetById(id);
            if (entity == null) return false;

            //  Validación crítica: no se permite modificar si hay una elección activa
            if (!await CanModify()) return false;

            entity.Estado = entity.Estado == EstadoEntidad.Activo
                            ? EstadoEntidad.Inactivo
                            : EstadoEntidad.Activo;

            await _puestoElectivoRepository.UpdateAsync(entity.Id, entity);
            return true;
        }

        public async Task<bool> CanModify()
        {
            //  Verifica si hay una elección en curso (Estado = EnProceso)
            var query = _eleccionRepository.GetAllQuery();
            var hayEleccionActiva = await query.AnyAsync(e => e.Estado == EstadoEleccion.EnProceso);

            //  Si hay elección activa, se bloquea el mantenimiento
            return !hayEleccionActiva;
        }

        public override async Task<PuestoElectivoDto?> AddAsync(PuestoElectivoDto dto)
        {
            //  Bloqueo absoluto si hay elección activa
            if (!await CanModify()) return null;

            var entity = _mapper.Map<PuestoElectivo>(dto);
            entity.Estado = EstadoEntidad.Activo; //  Fuerza estado activo al crear

            var saved = await _puestoElectivoRepository.AddAsync(entity);
            if (saved == null) return null;

            return _mapper.Map<PuestoElectivoDto>(saved);
        }

        public override async Task<PuestoElectivoDto?> UpdateAsync(PuestoElectivoDto dto, int id)
        {
            try
            {
                var actual = await _puestoElectivoRepository.GetById(id);
                if (actual == null) return null;

                var entity = _mapper.Map<PuestoElectivo>(dto);

                //  Preserva el estado actual para evitar sobrescritura accidental
                entity.Estado = actual.Estado;

                var updated = await _puestoElectivoRepository.UpdateAsync(id, entity);
                if (updated == null) return null;

                return _mapper.Map<PuestoElectivoDto>(updated);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            //  Bloqueo absoluto si hay elección activa
            if (!await CanModify()) return false;

            return await base.DeleteAsync(id);
        }

        public async Task<List<SelectListItem>> GetDisponiblesParaDirigente(int? puestoActualId = null)
        {
            var puestos = await _puestoElectivoRepository.GetAllList();
            var disponibles = new List<SelectListItem>();

            foreach (var puesto in puestos)
            {
                bool ocupado = await _candidatoRepository.ExisteCandidatoEnPuesto(_userSession.GetPartidoId(), puesto.Id);

                if (!ocupado || puesto.Id == puestoActualId)
                {
                    disponibles.Add(new SelectListItem
                    {
                        Value = puesto.Id.ToString(),
                        Text = puesto.Nombre
                    });
                }
            }

            return disponibles;
        }
    }

}

using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Services.Dirigente
{
    public class AlianzaPoliticaService : GenericService<AlianzaPolitica, AlianzaSolicitudEnviadaDto>, IAlianzaPoliticaService
    {
        private readonly IAlianzaPoliticaRepository _alianzaRepository;
        private readonly IPartidoPoliticoRepository _partidoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IMapper _mapper;

        public AlianzaPoliticaService(
            IAlianzaPoliticaRepository alianzaRepository,
            IPartidoPoliticoRepository partidoRepository,
            IEleccionRepository eleccionRepository,
            IMapper mapper
        ) : base(alianzaRepository, mapper)
        {
            _alianzaRepository = alianzaRepository;
            _partidoRepository = partidoRepository;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
        }

        public async Task<List<AlianzaSolicitudRecibidaDto>> GetSolicitudesRecibidasAsync(int partidoId)
        {
            var solicitudes = await _alianzaRepository.GetSolicitudesRecibidasAsync(partidoId);
            return _mapper.Map<List<AlianzaSolicitudRecibidaDto>>(solicitudes);
        }

        public async Task<List<AlianzaSolicitudEnviadaDto>> GetSolicitudesEnviadasAsync(int partidoId)
        {
            var solicitudes = await _alianzaRepository.GetSolicitudesEnviadasAsync(partidoId);
            return _mapper.Map<List<AlianzaSolicitudEnviadaDto>>(solicitudes);
        }

        public async Task<List<AlianzaVigenteDto>> GetAlianzasVigentesAsync(int partidoId)
        {
            var alianzas = await _alianzaRepository.GetAlianzasVigentesAsync(partidoId);

            var dtos = alianzas
                .Where(a => a.Estado == EstadoAlianza.Aceptada && a.FechaExpiracion > DateTime.Now)
                .Select(a =>
                {
                    var aliado = a.PartidoSolicitanteId == partidoId ? a.PartidoReceptor : a.PartidoSolicitante;
                    return new AlianzaVigenteDto
                    {
                        Id = a.Id,
                        PartidoAliadoId = aliado.Id,
                        NombrePartidoAliado = aliado.Nombre,
                        SiglasPartidoAliado = aliado.Siglas,
                        FechaAceptacion = a.FechaRespuesta!.Value,
                        FechaExpiracion = a.FechaExpiracion!.Value
                    };
                }).ToList();

            return dtos;
        }

        public async Task<bool> CrearSolicitudAsync(int partidoSolicitanteId, int partidoReceptorId)
        {
            if (!await CanModifyAsync()) return false;

            var nueva = new AlianzaPolitica
            {
                PartidoSolicitanteId = partidoSolicitanteId,
                PartidoReceptorId = partidoReceptorId,
                FechaSolicitud = DateTime.Now,
                Estado = EstadoAlianza.EnEspera
            };

            var resultado = await _alianzaRepository.AddAsync(nueva);
            return resultado != null;
        }

        public async Task<ConfirmarAccionAlianzaDto?> GetConfirmacionAceptarAsync(int solicitudId)
        {
            var solicitud = await _alianzaRepository.GetById(solicitudId);
            return solicitud == null ? null : _mapper.Map<ConfirmarAccionAlianzaDto>(solicitud);
        }

        public async Task<bool> AceptarSolicitudAsync(int solicitudId)
        {
            if (!await CanModifyAsync()) return false;

            var solicitud = await _alianzaRepository.GetById(solicitudId);
            if (solicitud == null || solicitud.Estado != EstadoAlianza.EnEspera) return false;

            solicitud.Estado = EstadoAlianza.Aceptada;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.FechaExpiracion = DateTime.Now.AddMonths(6); // ← Aquí defines la vigencia

            var actualizado = await _alianzaRepository.UpdateAsync(solicitud.Id, solicitud);
            return actualizado != null;
        }

        public async Task<ConfirmarAccionAlianzaDto?> GetConfirmacionRechazoAsync(int solicitudId)
        {
            var solicitud = await _alianzaRepository.GetById(solicitudId);
            return solicitud == null ? null : _mapper.Map<ConfirmarAccionAlianzaDto>(solicitud);
        }

        public async Task<bool> RechazarSolicitudAsync(int solicitudId)
        {
            if (!await CanModifyAsync()) return false;

            var solicitud = await _alianzaRepository.GetById(solicitudId);
            if (solicitud == null || solicitud.Estado != EstadoAlianza.EnEspera) return false;

            solicitud.Estado = EstadoAlianza.Rechazada;
            solicitud.FechaRespuesta = DateTime.Now;

            var actualizado = await _alianzaRepository.UpdateAsync(solicitud.Id, solicitud);
            return actualizado != null;
        }

        public async Task<ConfirmarAccionAlianzaDto?> GetConfirmacionEliminarAsync(int solicitudId)
        {
            var solicitud = await _alianzaRepository.GetById(solicitudId);
            return solicitud == null ? null : _mapper.Map<ConfirmarAccionAlianzaDto>(solicitud);
        }

        public async Task<bool> EliminarSolicitudAsync(int solicitudId)
        {
            if (!await CanModifyAsync()) return false;

            var solicitud = await _alianzaRepository.GetById(solicitudId);
            if (solicitud == null || solicitud.Estado != EstadoAlianza.EnEspera) return false;

            await _alianzaRepository.DeleteAsync(solicitud.Id);
            return true;
        }

        public async Task<bool> CanModifyAsync()
        {
            var eleccionActiva = await _eleccionRepository.GetEleccionActiva();
            return eleccionActiva == null;
        }

        async Task<List<ViewModels.OpcionItemViewModel>> IAlianzaPoliticaService.GetPartidosDisponiblesParaAlianza(int partidoId)
        {
            var todos = await _partidoRepository.GetActivos();
            var disponibles = new List<ViewModels.OpcionItemViewModel>();

            foreach (var partido in todos)
            {
                if (partido.Id == partidoId) continue;

                bool yaAliado = await _alianzaRepository.ExisteAlianzaActivaAsync(partidoId, partido.Id);
                bool pendiente = await _alianzaRepository.ExisteRelacionPendienteAsync(partidoId, partido.Id);

                if (!yaAliado && !pendiente)
                {
                    disponibles.Add(new ViewModels.OpcionItemViewModel
                    {
                        Id = partido.Id,
                        Nombre = $"{partido.Nombre} ({partido.Siglas})"
                    });
                }
            }

            return disponibles;

        }

        public async Task<List<int>> GetAliadosVigentesIdsAsync(int partidoId)
        {
            var alianzas = await _alianzaRepository.GetAlianzasVigentesAsync(partidoId);

            return alianzas
                .Where(a => a.Estado == EstadoAlianza.Aceptada && a.FechaExpiracion > DateTime.Now)
                .Select(a =>
                    a.PartidoSolicitanteId == partidoId
                        ? a.PartidoReceptorId
                        : a.PartidoSolicitanteId
                )
                .Distinct()
                .ToList();
        }

    }
}

using AutoMapper;
using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Services.Administrador
{
    public class DirigentePartidoService : GenericService<DirigentePartido, DirigentePartidoDto>, IDirigentePartidoService
    {
        private readonly IDirigentePartidoRepository _dirigentePartidoRepository;

        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        public DirigentePartidoService(
            IDirigentePartidoRepository dirigentePartidoRepository,
            IMapper mapper
            ,
            IUsuarioRepository usuarioRepository

        ) : base(dirigentePartidoRepository, mapper)
        {
            _dirigentePartidoRepository = dirigentePartidoRepository;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public async Task AsignarDirigenteAPartido(int usuarioId, int partidoId)
        {
            var relacion = new DirigentePartido
            {
                UsuarioId = usuarioId,
                PartidoPoliticoId = partidoId
            };
            await _dirigentePartidoRepository.AddAsync(relacion);

            await _usuarioRepository.AsignarPartido(usuarioId, partidoId);
        }

        public async Task<bool> DesvincularAsync(int relacionId)
        {
            var relacion = await _dirigentePartidoRepository.GetById(relacionId);
            if (relacion == null) return false;

            await _dirigentePartidoRepository.DeleteAsync(relacion.Id); //  no se asigna
            return true;
        }

        public async Task<bool> CanModify()
        {
            // Por ahora, siempre permite modificar
            return true;
        }

        public async Task<List<int>> GetIdsDirigentesAsignados()
        {
            return await _dirigentePartidoRepository.GetIdsDirigentesAsignados();
        }

        public async Task<bool> DirigenteYaAsignado(int usuarioId)
        {
            return await _dirigentePartidoRepository.DirigenteYaAsignado(usuarioId);
        }

        public async Task<List<DirigentePartidoDto>> GetAllWithIncludes()
        {
            var list = await _dirigentePartidoRepository.GetAllWithIncludes();
            return _mapper.Map<List<DirigentePartidoDto>>(list);
        }

    
    }
}

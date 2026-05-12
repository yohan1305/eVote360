using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class AsignacionCandidatoPuestoRepository
      : GenericRepository<AsignacionCandidatoPuesto>, IAsignacionCandidatoPuestoRepository
    {
        private readonly Evote365DbContext _context;

        public AsignacionCandidatoPuestoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteAsignacionAsync(int candidatoId, int partidoId, int puestoId)
        {
            return await _context.AsignacionesCandidatosPuestos
                .AnyAsync(a =>
                    a.CandidatoId == candidatoId &&
                    a.PartidoPoliticoId == partidoId &&
                    a.PuestoElectivoId == puestoId);
        }

        public async Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorCandidatoAsync(int candidatoId)
        {
            return await _context.AsignacionesCandidatosPuestos
                .Where(a => a.CandidatoId == candidatoId)
                .Include(a => a.PartidoPolitico)
                .Include(a => a.PuestoElectivo)
                .ToListAsync();
        }
        public async Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorPartidoAsync(int partidoId)
        {
            return await _context.AsignacionesCandidatosPuestos
                .AsNoTracking() // Recomendado para consultas de solo lectura (validaciones)
                .Include(a => a.Candidato)
                .Include(a => a.PuestoElectivo)
                .Include(a => a.PartidoPolitico)
                .Where(a => a.PartidoPoliticoId == partidoId)
                .ToListAsync();
        }

        public async Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorCandidatosAsync(List<int> candidatosIds)
        {
            return await _context.AsignacionesCandidatosPuestos
                .Include(a => a.PuestoElectivo)
                .Where(a => candidatosIds.Contains(a.CandidatoId))
                .ToListAsync();
        }

        public async Task<List<AsignacionCandidatoPuesto>> GetAsignacionesPorPuestoAsync(int puestoId)
        {
            return await _context.AsignacionesCandidatosPuestos
                .Where(a => a.PuestoElectivoId == puestoId)
                .Include(a => a.Candidato)
                .Include(a => a.PartidoPolitico)
                .ToListAsync();
        }
    }
}

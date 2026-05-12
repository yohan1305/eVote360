using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class AlianzaPoliticaRepository : GenericRepository<AlianzaPolitica>, IAlianzaPoliticaRepository
    {

        private readonly Evote365DbContext _context;
        public AlianzaPoliticaRepository(Evote365DbContext context) : base(context)
        {

            _context = context;
        }

        public async Task<List<AlianzaPolitica>> GetSolicitudesRecibidasAsync(int partidoId)
        {
            return await _context.AlianzasPoliticas
                .Include(a => a.PartidoSolicitante)
                .Where(a => a.PartidoReceptorId == partidoId && a.Estado == EstadoAlianza.EnEspera)
                .ToListAsync();
        }

        public async Task<List<AlianzaPolitica>> GetSolicitudesEnviadasAsync(int partidoId)
        {
            return await _context.AlianzasPoliticas
                .Include(a => a.PartidoReceptor)
                .Where(a => a.PartidoSolicitanteId == partidoId)
                .ToListAsync();
        }

        public async Task<List<AlianzaPolitica>> GetAlianzasVigentesAsync(int partidoId)
        {
            return await _context.AlianzasPoliticas
                .Include(a => a.PartidoSolicitante)
                .Include(a => a.PartidoReceptor)
                .Where(a => a.Estado == EstadoAlianza.Aceptada &&
                            (a.PartidoSolicitanteId == partidoId || a.PartidoReceptorId == partidoId))
                .ToListAsync();
        }

        public async Task<bool> ExisteRelacionPendienteAsync(int partidoAId, int partidoBId)
        {
            return await _context.AlianzasPoliticas.AnyAsync(a =>
                a.Estado == EstadoAlianza.EnEspera &&
                ((a.PartidoSolicitanteId == partidoAId && a.PartidoReceptorId == partidoBId) ||
                 (a.PartidoSolicitanteId == partidoBId && a.PartidoReceptorId == partidoAId)));
        }

        public async Task<bool> ExisteAlianzaActivaAsync(int partidoAId, int partidoBId)
        {
            return await _context.AlianzasPoliticas.AnyAsync(a =>
                a.Estado == EstadoAlianza.Aceptada &&
                ((a.PartidoSolicitanteId == partidoAId && a.PartidoReceptorId == partidoBId) ||
                 (a.PartidoSolicitanteId == partidoBId && a.PartidoReceptorId == partidoAId)));
        }

        public async Task<bool> ExisteAlianzaEntre(int partidoAId, int partidoBId)
        {
            return await _context.AlianzasPoliticas
                .AnyAsync(a =>
                    a.Estado == EstadoAlianza.Aceptada &&
                    (
                        (a.PartidoSolicitanteId == partidoAId && a.PartidoReceptorId == partidoBId) ||
                        (a.PartidoSolicitanteId == partidoBId && a.PartidoReceptorId == partidoAId)
                    )
                );
        }

        public async Task<List<int>> GetAliadosVigentesIdsAsync(int partidoId)
        {
            var alianzas = await _context.AlianzasPoliticas
                .AsNoTracking()
                .Where(a =>
                    a.Estado == EstadoAlianza.Aceptada &&
                    (!a.FechaExpiracion.HasValue || a.FechaExpiracion > DateTime.Now) &&
                    (a.PartidoSolicitanteId == partidoId || a.PartidoReceptorId == partidoId))
                .ToListAsync();

            var aliadosIds = alianzas
                .Select(a => a.PartidoSolicitanteId == partidoId ? a.PartidoReceptorId : a.PartidoSolicitanteId)
                .Distinct()
                .ToList();

            return aliadosIds;
        }

    }
}

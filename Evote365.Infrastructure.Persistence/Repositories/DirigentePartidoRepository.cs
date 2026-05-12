using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class DirigentePartidoRepository : GenericRepository<DirigentePartido>, IDirigentePartidoRepository
    {
        private readonly Evote365DbContext _context;

        public DirigentePartidoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DirigenteYaAsignado(int usuarioId)
        {
            return await _context.DirigentesPartidos
                .AnyAsync(dp => dp.UsuarioId == usuarioId);
        }

        public async Task<List<DirigentePartido>> GetAllWithIncludes()
        {
            return await _context.DirigentesPartidos
                .Include(dp => dp.Usuario)
                .Include(dp => dp.PartidoPolitico)
                .ToListAsync();
        }

        public async Task<List<int>> GetIdsDirigentesAsignados()
        {
            var query = _context.DirigentesPartidos
                .Select(dp => dp.UsuarioId);

            return await query.ToListAsync();
        }


    }
}

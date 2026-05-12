using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class PartidoPoliticoRepository : GenericRepository<PartidoPolitico>, IPartidoPoliticoRepository
    {
        private readonly Evote365DbContext _context;
        public PartidoPoliticoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PartidoPolitico>> GetActivos()
        {
            return await _context.PartidosPoliticos
                .Where(p => p.Estado == EstadoEntidad.Activo)
                .ToListAsync();
        }
    }
}

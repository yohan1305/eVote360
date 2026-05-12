using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class EleccionPuestoRepository: GenericRepository<EleccionPuesto>, IEleccionPuestoRepository
    {
        public EleccionPuestoRepository(Evote365DbContext context) : base(context)
        {
        }
    }
  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Interfaces
{
    public interface IGenericService<DtoModel>
        where DtoModel : class
    {
        Task<DtoModel?> AddAsync(DtoModel dto);
        Task<DtoModel?> UpdateAsync(DtoModel dto, int id);
        Task<bool> DeleteAsync(int id);
        Task<DtoModel?> GetById(int id);
        Task<List<DtoModel>> GetAll();
    }
}

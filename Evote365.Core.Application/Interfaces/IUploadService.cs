using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Evote365.Core.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}

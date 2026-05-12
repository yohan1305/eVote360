using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.Interfaces.Elector;
using Evote365.Core.Application.Services.Administrador;
using Evote365.Core.Application.Services.Dirigente;
using Evote365.Core.Application.Services.Elector;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Evote365.Core.Application
{
    public static class ServicesRegistration
    {
        // Extension method - Decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region Services IOC
            services.AddTransient<IPuestoElectivoService, PuestoElectivoService>();
            services.AddTransient<IPartidoPoliticoService, PartidoPoliticoService>();
            services.AddTransient<ICiudadanoService, CiudadanoService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IDirigentePartidoService, DirigentePartidoService>();
            services.AddTransient<IEleccionService, EleccionService>();
            services.AddTransient<ICandidatoService, CandidatoService>();
            services.AddTransient<IAsignacionCandidatoPuestoService,AsignacionCandidatoPuestoService>();
            services.AddTransient<IAlianzaPoliticaService, AlianzaPoliticaService>();
            services.AddTransient<IElectorService, ElectorService>();
            #endregion
        }

    }
}

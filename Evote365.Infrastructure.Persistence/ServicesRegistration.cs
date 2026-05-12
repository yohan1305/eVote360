using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Infrastructure.Persistence.Context;
using Evote365.Infrastructure.Persistence.Repositories;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evote365.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        // Extension method - Decorator pattern
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Context
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<Evote365DbContext>(opt =>
                    opt.UseInMemoryDatabase("EvoteDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<Evote365DbContext>(opt =>
                    opt.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(typeof(Evote365DbContext).Assembly.FullName)),
                    ServiceLifetime.Transient);
            }
            #endregion

            #region Repositories IOC
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
            services.AddTransient<ICandidatoRepository, CandidatoRepository>();
            services.AddTransient<IAlianzaPoliticaRepository, AlianzaPoliticaRepository>();
            services.AddTransient<IVotoRepository, VotoRepository>();
            services.AddTransient<ICiudadanoRepository, CiudadanoRepository>();
            services.AddTransient<IPuestoElectivoRepository, PuestoElectivoRepository>();
            services.AddTransient<IEleccionRepository, EleccionRepository>();
            services.AddTransient<IEleccionPuestoRepository, EleccionPuestoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IDirigentePartidoRepository, DirigentePartidoRepository>();
            services.AddTransient<IAsignacionCandidatoPuestoRepository, AsignacionCandidatoPuestoRepository>();
            #endregion
        }

    }
}

using Evote365.Core.Application.Helpers;
using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Seed
{
    public static class DatabaseInitializer
    {
        public static async Task SeedAsync(Evote365DbContext context)
        {
            await context.Database.MigrateAsync();
            await SeedDefaultAdminUserAsync(context);
        }

        private static async Task SeedDefaultAdminUserAsync(Evote365DbContext context)
        {
            const string username = "admin";

            var exists = await context.Usuarios.AnyAsync(u => u.NombreUsuario == username);
            if (exists)
            {
                return;
            }

            var adminUser = new Usuario
            {
                Nombre = "Administrador",
                Apellido = "Sistema",
                Email = "admin@evote365.local",
                NombreUsuario = username,
                PasswordHash = PasswordEncryption.Hash("123Pa$$word!"),
                Rol = RolUsuario.Administrador,
                Estado = EstadoEntidad.Activo
            };

            await context.Usuarios.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}

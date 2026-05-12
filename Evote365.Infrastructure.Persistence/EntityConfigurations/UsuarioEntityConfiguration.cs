using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Infrastructure.Persistence.EntityConfigurations
{
    public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Usuarios");
            #endregion

            #region Property configurations
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.NombreUsuario).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Rol).IsRequired();
            builder.Property(u => u.Estado).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            #endregion

            #region Relationships
            builder.HasOne(u => u.PartidoAsignado)
                   .WithMany(p => p.Dirigentes)
                   .HasForeignKey(u => u.PartidoAsignadoId)
                   .OnDelete(DeleteBehavior.SetNull);
            #endregion
        }
    }

}

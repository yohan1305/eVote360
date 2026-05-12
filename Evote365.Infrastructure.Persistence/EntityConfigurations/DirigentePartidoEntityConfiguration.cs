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
    public class DirigentePartidoEntityConfiguration : IEntityTypeConfiguration<DirigentePartido>
    {
        public void Configure(EntityTypeBuilder<DirigentePartido> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("DirigentesPartidos");
            #endregion

            #region Relationships
            builder.HasOne(dp => dp.Usuario)
                   .WithMany()
                   .HasForeignKey(dp => dp.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict); // Evita cascada si se borra el usuario

            builder.HasOne(dp => dp.PartidoPolitico)
                   .WithMany()
                   .HasForeignKey(dp => dp.PartidoPoliticoId)
                   .OnDelete(DeleteBehavior.Restrict); // Evita cascada si se borra el partido
            #endregion
        }
    }
}

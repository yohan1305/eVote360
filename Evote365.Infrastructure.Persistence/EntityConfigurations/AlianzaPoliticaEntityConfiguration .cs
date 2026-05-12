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
    public class AlianzaPoliticaEntityConfiguration : IEntityTypeConfiguration<AlianzaPolitica>
    {
        public void Configure(EntityTypeBuilder<AlianzaPolitica> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("AlianzasPoliticas");
            #endregion

            #region Property configurations
            builder.Property(a => a.Estado).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(a => a.PartidoSolicitante)
                   .WithMany(p => p.AlianzasEnviadas)
                   .HasForeignKey(a => a.PartidoSolicitanteId)
                   .OnDelete(DeleteBehavior.NoAction); // ← cambio aquí

            builder.HasOne(a => a.PartidoReceptor)
                   .WithMany(p => p.AlianzasRecibidas)
                   .HasForeignKey(a => a.PartidoReceptorId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}

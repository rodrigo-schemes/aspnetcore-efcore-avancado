using EFCoreModulo08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModulo08.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder
                .HasOne(p=>p.Governador)
                .WithOne(p=>p.Estado)
                .HasForeignKey<Governador>(p=>p.EstadoId);

            // Auto include da entidade filho na consulta
            builder.Navigation(p=>p.Governador).AutoInclude();
            
            builder
                .HasMany(p=>p.Cidades)
                .WithOne(p=>p.Estado)
                .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
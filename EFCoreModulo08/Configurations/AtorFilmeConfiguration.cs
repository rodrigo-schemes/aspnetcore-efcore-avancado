using EFCoreModulo08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModulo08.Configurations;

public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
{
    public void Configure(EntityTypeBuilder<Ator> builder)
    {
        // Configuração Muitos Para Muitos Simples
        // builder
        //     .HasMany(p=>p.Filmes)
        //     .WithMany(p=>p.Atores)
        //     .UsingEntity(p=>p.ToTable("AtoresFilmes"));

        // Configuração Muitos Para Muitos Customizada
        builder
            .HasMany(p=>p.Filmes)
            .WithMany(p=>p.Atores)
            .UsingEntity<Dictionary<string,object>>(
                "FilmesAtores",
                p=>p.HasOne<Filme>().WithMany().HasForeignKey("FilmeId"),
                p=>p.HasOne<Ator>().WithMany().HasForeignKey("AtorId"),
                p=> 
                {
                    p.Property<DateTime>("CadastradoEm").HasDefaultValueSql("GETDATE()");
                }
            );
    }
}
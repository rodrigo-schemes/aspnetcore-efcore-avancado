using EFCoreModulo08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModulo08.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.OwnsOne(x=>x.Endereco, end => 
        {
            end.Property(p=>p.Bairro).HasColumnName("Bairro");
            end.Property(p=>p.Cidade).HasColumnName("Cidade");
            end.Property(p=>p.Estado).HasColumnName("UF");
            end.Property(p => p.Logradouro).HasColumnName("Logradouro");

            // criará uma tabela Enderecos, caso contrário a model Endereco será criadas na tabela Clientes
            end.ToTable("Enderecos");
        });
    }
}
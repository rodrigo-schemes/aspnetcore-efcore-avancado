using EFCoreModulo08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreModulo08.Configurations
{
    public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.Property("_cpf").HasColumnName("CPF").HasMaxLength(11);
            //.HasField("_cpf");
        }
    }
}
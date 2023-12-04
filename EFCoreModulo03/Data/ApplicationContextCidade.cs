using EFCoreModulo03.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo03.Data;

public class ApplicationContextCidade : DbContext
{
    public DbSet<Cidade> Cidades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Server=localhost,1433;Database=CursoEFCoreAvancado;User Id=sa;Password=P@ssword!;TrustServerCertificate=true";

        optionsBuilder
            .UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
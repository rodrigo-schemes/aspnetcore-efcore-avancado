using EFCoreModulo11.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo11.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Funcao> Funcoes {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .AddInterceptors(new Interceptadores.InterceptadorDeComandos())
            .AddInterceptors(new Interceptadores.InterceptadorDeConexao())
            .AddInterceptors(new Interceptadores.InterceptadorPersistencia());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Funcao>(conf=>
            {
                conf.Property<string>("PropriedadeSombra")
                    .HasColumnType("VARCHAR(100)")
                    .HasDefaultValueSql("'Teste'");
            });
    }
}
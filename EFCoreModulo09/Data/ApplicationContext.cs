using EFCoreModulo09.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo09.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Atributo> Atributos { get; set; }
    public DbSet<Aeroporto> Aeroportos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<RelatorioFinanceiro> Relatorios { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection) 
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
using EFCoreModulo03.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo03.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            //.UseLazyLoadingProxies() //habilita lazy loading de forma global
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
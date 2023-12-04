using EFCoreModulo22.Entities;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCoreModulo22.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(Setup.GetConnectionString())
            .EnableSensitiveDataLogging();
    }
}
using EFCoreModulo15.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo15.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Pessoa> Pessoas {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(conf=> 
        {
            conf.HasKey(p=>p.Id);
            conf.Property(p=>p.Nome).HasMaxLength(60).IsUnicode(false);
        });
    }
}
using EFCoreModulo12.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo12.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Livro> Livros {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection)
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging();
    }
}
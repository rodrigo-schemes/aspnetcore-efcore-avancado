using EFCoreModulo16.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo16.Data;

public class ApplicationContext(EnumDatabase database) : DbContext
{
    public DbSet<Pessoa> Pessoas {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ObterBancoDeDados(optionsBuilder, database)
            ?.LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(conf=> 
        {
            conf.HasKey(p=>p.Id);
            conf.Property(p=>p.Nome).HasMaxLength(60).IsUnicode(false);

            // necessário para cosmos db
            if (database == EnumDatabase.COSMOSDB)
            {
                conf.ToContainer("Pessoas");    
            }
        });
    }

    private static DbContextOptionsBuilder? ObterBancoDeDados(DbContextOptionsBuilder optionsBuilder, EnumDatabase database)
    {
        return database switch
        {
            EnumDatabase.SQLSERVER => optionsBuilder.UseSqlServer(Setup.GetConnectionString()),
            EnumDatabase.POSTGRES => optionsBuilder.UseNpgsql(
                "Host=localhost;Database=DEVIO04;Username=postgres;Password=P@ssword!"),
            EnumDatabase.SQLITE => optionsBuilder.UseSqlite("Data source=devio04.db"),
            EnumDatabase.INMEMORY => optionsBuilder.UseInMemoryDatabase(databaseName: "teste-devio"),
            EnumDatabase.COSMOSDB => optionsBuilder.UseCosmos("https://localhost:8081", 
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", 
                "dev-io04"),
            _ => default
        };
    }
}

public enum EnumDatabase
{
    SQLSERVER,
    POSTGRES,
    SQLITE,
    INMEMORY,
    COSMOSDB
}
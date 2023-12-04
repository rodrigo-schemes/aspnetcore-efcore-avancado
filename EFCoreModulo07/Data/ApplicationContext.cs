using EFCoreModulo07.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo07.Data;

public class ApplicationContext : DbContext
{
    private readonly StreamWriter _writer = new("meulog.txt", append: true);
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var strConnection = Setup.GetConnectionString();

        optionsBuilder
            .UseSqlServer(strConnection) 
            .LogTo(Console.WriteLine, LogLevel.Information);
        
        // Configurar Log Simplificado
        // optionsBuilder
        //     .UseSqlServer(strConnection)
        //     .LogTo(Console.WriteLine, LogLevel.Information);
        
        // Filtrar Eventos de Log
        // optionsBuilder
        //     .UseSqlServer(strConnection)
        //     .LogTo(Console.WriteLine, new []{ CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted }, 
        //         LogLevel.Information, DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine);
        
        // Escrever log em arquivo (grava na pasta bin)
        // optionsBuilder
        //     .UseSqlServer(strConnection)
        //     .LogTo(_writer.WriteLine, LogLevel.Information);
        
        // Habilitando erros detalhados (indica qual propriedade da model ocorreu erro)
        // optionsBuilder
        //     .UseSqlServer(strConnection)
        //     .EnableDetailedErrors();
        
        // Habilitar visualização de dados sensíveis
        // optionsBuilder
        //     .UseSqlServer(strConnection)
        //     .LogTo(Console.WriteLine, LogLevel.Information)
        //     .EnableSensitiveDataLogging();
        
        // Configurar o batch size (Padrão em 42 registros)
        // optionsBuilder
        //     .UseSqlServer(strConnection, opt => opt.MaxBatchSize(100))
        //     .LogTo(Console.WriteLine, LogLevel.Information);
        
        // Configurar o timeout do comando global
        // optionsBuilder
        //     .UseSqlServer(strConnection, opt => opt.CommandTimeout(5))
        //     .LogTo(Console.WriteLine, LogLevel.Information);
        
        // Configurar resiliência
        // optionsBuilder
        //     .UseSqlServer(strConnection, opt => 
        //         opt.EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null ))
        //     .LogTo(Console.WriteLine, LogLevel.Information);
    }

    public override void Dispose()
    {
        base.Dispose();
        _writer.Dispose();
    }
}
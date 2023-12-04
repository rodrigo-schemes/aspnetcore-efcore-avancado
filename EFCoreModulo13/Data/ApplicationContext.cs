using System.Reflection;
using EFCoreModulo13.Domain;
using EFCoreModulo13.Funcoes;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace EFCoreModulo13.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Livro> Livros {get;set;}

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
        //Curso.Funcoes.MinhasFuncoes.RegistarFuncoes(modelBuilder);

        modelBuilder
            .HasDbFunction(MinhaFuncao!)
            .HasName("LEFT")
            .IsBuiltIn();

        modelBuilder
            .HasDbFunction(LetrasMaiusculas!)
            .HasName("ConverterParaLetrasMaiusculas")
            .HasSchema("dbo");

        modelBuilder
            .HasDbFunction(DateDiff!)
            .HasName("DATEDIFF")
            .HasTranslation(p=> 
            {
                var argumentos = p.ToList();

                var contante = (SqlConstantExpression)argumentos[0];
                argumentos[0] = new SqlFragmentExpression(contante.Value.ToString());

                return new SqlFunctionExpression("DATEDIFF", argumentos, false, new[]{false, false, false}, typeof(int), null);

            })
            .IsBuiltIn();
    }

    private static readonly MethodInfo? MinhaFuncao = typeof(MinhasFuncoes)
        .GetRuntimeMethod("Left", new[] {typeof(string), typeof(int)});

    private static readonly MethodInfo? LetrasMaiusculas = typeof(MinhasFuncoes)
        .GetRuntimeMethod(nameof(MinhasFuncoes.LetrasMaiusculas), new[] {typeof(string)});

    private static readonly MethodInfo? DateDiff = typeof(MinhasFuncoes)
        .GetRuntimeMethod(nameof(MinhasFuncoes.DateDiff), new[] {typeof(string), typeof(DateTime), typeof(DateTime)});
    
}
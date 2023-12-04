using System.Diagnostics;
using EFCoreModulo19.Data;
using EFCoreModulo19.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo19;

internal static class Program
{
    private static void Main()
    {
        Setup.CriarDatabaseParaExemplos(new ApplicationContext());
        
        ToQueryString();
        DebugView();
        Clear();
        ConsultaFiltrada();
        SingleOrDefaultVsFirstOrDefault();
        SemChavePrimaria();
        UsarView();
        NaoUnicode();
        OperadoresDeAgregacao();
        OperadoresDeAgregacaoNoAgrupamento();
        ContadorDeEventos();
    }

    private static void ToQueryString()
    {
        using var db = new ApplicationContext();
        var query = db.Departamentos.Where(p=>p.Id > 2);
        
        //Obtem a query SQL gerada
        var sql = query.ToQueryString();
        
        Console.WriteLine(sql);
    }

    private static void DebugView()
    {
        using var db = new ApplicationContext();
        db.Departamentos.Add(new Departamento{ Descricao = "TESTE DebugView"});

        //Em modo debug, observar a propriedade Debug View
        var query = db.Departamentos.Where(p=>p.Id > 2);
    }

    private static void Clear()
    {
        using var db = new ApplicationContext();

        db.Departamentos.Add(new Departamento{ Descricao = "TESTE DebugView"});

        // Descarta as mudanças nos objetos rastreados pelo contexto
        db.ChangeTracker.Clear();
    }

    private static void ConsultaFiltrada()
    {
        using var db = new ApplicationContext();

        // Trazer o relacionamento já filtrado
        var sql = db
            .Departamentos
            .Include(p=>p.Colaboradores.Where(c=>c.Nome.Contains("Teste")))
            .ToQueryString();

        Console.WriteLine(sql);
    }

    static void SingleOrDefaultVsFirstOrDefault()
    {
        using var db = new ApplicationContext(); 

        // SingleOrDefault realiza um TOP 2. Caso venha dois registros, dispara execeção
        // Bom uso em casos de CPF's duplicados por exemplo
        Console.WriteLine("SingleOrDefault:");
        _ = db.Departamentos.SingleOrDefault(p=>p.Id > 2);

        // FirstOrDefault traz o primeiro resultado
        Console.WriteLine("FirstOrDefault:");
        _ = db.Departamentos.FirstOrDefault(p=>p.Id > 2);
    }

    private static void SemChavePrimaria()
    {
        using var db = new ApplicationContext();
        var usuarioFuncoes = db.UsuarioFuncoes.Where(p=>p.UsuarioId == Guid.NewGuid()).ToArray();
    }

    private static void UsarView()
    {
        using var db = new ApplicationContext();
            
        db.Database.ExecuteSqlRaw(
            """
            CREATE VIEW vw_departamento_relatorio AS
                            SELECT
                                d.Descricao, count(c.Id) as Colaboradores
                            FROM Departamentos d
                            LEFT JOIN Colaboradores c ON c.Departamento_Id=d.Id
                            GROUP BY d.Descricao
            """);

        var departamentos = Enumerable.Range(1,10)
            .Select(p=> new Departamento
            {
                Descricao = $"Departamento {p}",
                Colaboradores = Enumerable.Range(1, p)
                    .Select(c => new Colaborador
                    {
                        Nome = $"Colaborador {p}-{c}"
                    }).ToList()
            });

        var departamento = new Departamento
        {
            Descricao = $"Departamento Sem Colaborador"
        };

        db.Departamentos.Add(departamento);
        db.Departamentos.AddRange(departamentos);
        db.SaveChanges();

        var relatorio = db.DepartamentoRelatorio
            .Where(p=>p.Colaboradores < 20)
            .OrderBy(p=>p.Departamento)
            .ToList();

        foreach(var dep in relatorio)
        {
            Console.WriteLine($"{dep.Departamento} [ Colaboradores: {dep.Colaboradores}]");
        }
    }

    private static void NaoUnicode()
    {
        using var db = new ApplicationContext();

        var sql = db.Database.GenerateCreateScript();

        Console.WriteLine(sql);
    }

    private static void OperadoresDeAgregacao()
    {
        using var db = new ApplicationContext();

        var sql = db.Departamentos
            .GroupBy(p=>p.Descricao)
            .Select(p=>
                new 
                {
                    Descricao = p.Key,
                    Contador = p.Count(),
                    Media = p.Average(p=>p.Id),
                    Maximo = p.Max(p=>p.Id),
                    Soma = p.Sum(p=>p.Id)
                }).ToQueryString();

        Console.WriteLine(sql);
    }

    private static void OperadoresDeAgregacaoNoAgrupamento()
    {
        using var db = new ApplicationContext();

        // GROUP BY com HAVING
        var sql = db.Departamentos
            .GroupBy(p=>p.Descricao)
            .Where(p=>p.Count() > 1)
            .Select(p=>
                new 
                {
                    Descricao = p.Key,
                    Contador = p.Count()
                }).ToQueryString();

        Console.WriteLine(sql);
    }

    private static void ContadorDeEventos()
    {
        using var db = new ApplicationContext();
            
        Console.WriteLine($" PID: {Environment.ProcessId}");
        //Com o PID executar o comando no terminal
        //dotnet counters monitor -p [PID] Microsoft.EntityFrameworkCore

        while(Console.ReadKey().Key != ConsoleKey.Escape)
        {
            var departamento = new Departamento
            {
                Descricao = $"Departamento Sem Colaborador"
            };

            db.Departamentos.Add(departamento);
            db.SaveChanges();

            _ = db.Departamentos.Find(1);
            _ = db.Departamentos.AsNoTracking().FirstOrDefault();
        }
    }
}
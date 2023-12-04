using System.Diagnostics;
using EFCoreModulo14.Data;
using EFCoreModulo14.Domain;
using Microsoft.EntityFrameworkCore;

PopularRegistros();
ConsultaRastreada();
ConsultaNaoRastreada();
ConsultaComResolucaoDeIdentidade();
ConsultaCustomizada();
ConsultaProjetadaERastreada();
ConsultaProjetada();
return;

static void ConsultaRastreada()
{
    using var db = new ApplicationContext();

    var funcionarios = db.Funcionarios.Include(p => p.Departamento).ToList();
}

static void ConsultaNaoRastreada()
{
    using var db = new ApplicationContext();

    var funcionarios = db.Funcionarios.AsNoTracking().Include(p => p.Departamento).ToList();
}

static void ConsultaComResolucaoDeIdentidade()
{
    using var db = new ApplicationContext();

    // Consome menos memória que o AsNoTracking()
    var funcionarios = db.Funcionarios
        .AsNoTrackingWithIdentityResolution()
        .Include(p => p.Departamento)
        .ToList();
}

static void ConsultaCustomizada()
{
    using var db = new ApplicationContext();

    // alterar rastreamento no Context
    db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    
    var funcionarios = db.Funcionarios
        //.AsNoTrackingWithIdentityResolution() pode ser configurado por método de extensão
        .Include(p => p.Departamento)
        .ToList();
}

static void ConsultaProjetadaERastreada()
{
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos
        .Include(p => p.Funcionarios)
        .Select(p=> new 
        {
            Departamento = p,
            TotalFuncionarios = p.Funcionarios.Count
        })
        .ToList();

    departamentos[0].Departamento.Descricao = "Departamento Teste Atualizado";

    db.SaveChanges();
}

static void ConsultaProjetada()
{
    Inserir_200_Departamentos_Com_1MB();
    
    using var db = new ApplicationContext();

    //var departamentos = db.Departamentos.ToArray(); query sem projeção vai ocupar mais memória
    var departamentos = db.Departamentos.Select(p=> p.Descricao).ToArray();

    var memoria = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024 + " MB";

    Console.WriteLine(memoria);
}

static void PopularRegistros()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Departamentos.Add(new Departamento
    {
        Descricao = "Departamento Teste",
        Ativo = true,
        Funcionarios = Enumerable.Range(1, 100).Select(p => new Funcionario
        {
            CPF = p.ToString().PadLeft(11, '0'),
            Nome = $"Funcionando {p}",
            RG = p.ToString()
        }).ToList()
    });

    db.SaveChanges();
}

static void Inserir_200_Departamentos_Com_1MB()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    var random = new Random();
    
    db.Departamentos.AddRange(Enumerable.Range(1, 200).Select(p => 
        new Departamento
        {
            Descricao = "Departamento Teste",
            Image = getBytes()
        }));

    db.SaveChanges();

    byte[] getBytes()
    {
        var buffer = new byte[1024 * 1024];
        random.NextBytes(buffer);

        return buffer;
    }
}
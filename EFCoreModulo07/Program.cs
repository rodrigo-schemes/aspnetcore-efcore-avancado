using EFCoreModulo07.Data;
using EFCoreModulo07.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
PopularRegistros();

//Modulo 7
ConsultarComLogSimplificado();
ConsultaComDadosSensiveis();
HabilitandoBatchSize();
ConfigurarTimeout();
ExecutarEstrategiaResiliencia();

#region Modulo 7

static void ExecutarEstrategiaResiliencia()
{
    using var db = new ApplicationContext();

    //quando estiver habilitado a resiliência, risco de duplicar o registro em transações
    // através desse método, o EF consegue tornar transações resilientes
    var strategy = db.Database.CreateExecutionStrategy();
    strategy.Execute(() =>
    {
        using var transaction = db.Database.BeginTransaction();

        db.Departamentos.Add(new Departamento { Descricao = "Departamento Transacao"});
        db.SaveChanges();

        transaction.Commit();
    });
}

static void ConfigurarTimeout()
{
    using var db = new ApplicationContext();

    db.Database.SetCommandTimeout(10);

    db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07';SELECT 1");
}

static void HabilitandoBatchSize()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    for (var i = 0; i < 100; i++)
    {
        db.Departamentos.Add(
            new Departamento
            {
                Descricao = "Departamento " + i
            });
    }

    db.SaveChanges();
}

static void ConsultaComDadosSensiveis()
{
    using var db = new ApplicationContext();

    var descricao = "Departamento";
    var departamentos = db.Departamentos.Where(p => p.Descricao == descricao).ToArray();
}

static void ConsultarComLogSimplificado()
{
    // ver configuração no Application Context
    using var db = new ApplicationContext();
    
    var departamentos = db.Departamentos.Where(p => p.Id > 0).ToArray();
}

#endregion

#region Setup

static void PopularRegistros()
{
    using var db = new ApplicationContext();
    
    db.Departamentos.AddRange(
        new Departamento
        {
            Ativo = true,
            Descricao = "Departamento 01",
            Funcionarios = new List<Funcionario>
            {
                new()
                {
                    Nome = "Rafael Almeida",
                    CPF = "99999999911",
                    RG= "2100062"
                }
            },
            Excluido = true
        },
        new Departamento
        {
            Ativo = true,
            Descricao = "Departamento 02",
            Funcionarios = new List<Funcionario>
            {
                new()
                {
                    Nome = "Bruno Brito",
                    CPF = "88888888811",
                    RG= "3100062"
                },
                new()
                {
                    Nome = "Eduardo Pires",
                    CPF = "77777777711",
                    RG= "1100062"
                }
            }
        });

    db.SaveChanges();
    db.ChangeTracker.Clear();
}
#endregion
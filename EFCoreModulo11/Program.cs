using EFCoreModulo11.Data;
using EFCoreModulo11.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
Setup.GerarScripts(new ApplicationContext());
CriarRegistros();

//Modulo 11
TesteInterceptacao();
TesteInterceptacaoSaveChanges();

#region Modulo 11

static void TesteInterceptacaoSaveChanges()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Funcoes.Add(new Funcao
    {
        Descricao1 = "Teste"
    });

    db.SaveChanges();
}

static void TesteInterceptacao()
{
    using var db = new ApplicationContext();
    var consulta = db
        .Funcoes
        .TagWith("Use NOLOCK")
        .FirstOrDefault(); 

    Console.WriteLine($"Consulta: {consulta?.Descricao1}");
}

#endregion

#region Setup

static void CriarRegistros()
{
    using var db = new ApplicationContext();

    db.Funcoes.AddRange(
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(2),
            Data2 = "2021-01-01",
            Descricao1 = "Bala 1 ",
            Descricao2 = "Bala 1 "
        },
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(1),
            Data2 = "XX21-01-01",
            Descricao1 = "Bola 2",
            Descricao2 = "Bola 2"
        },
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(1),
            Data2 = "XX21-01-01",
            Descricao1 = "Tela",
            Descricao2 = "Tela"
        });

    db.SaveChanges();
}

#endregion
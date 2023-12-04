using System.Data;
using EFCoreModulo05.Data;
using EFCoreModulo05.Domain;
using EFCoreSetup;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
PopularDados();

//Modulo 5
FiltroGlobal();
IgnoreFiltroGlobal();
ConsultaProjetada();
ConsultaParametrizada();
ConsultaInterpolada();
ConsultaComTAG();
EntendendoConsulta1NN1();
DivisaoDeConsulta();


#region Modulo 5
static void DivisaoDeConsulta()
{
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos
        .Include(p => p.Funcionarios)
        .Where(p => p.Id < 3)
        .AsSplitQuery() // Realiza duas queries para tratar Explosão Cartesiana. Departamentos e INNER JOIN com Funcionarios
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");
        foreach (var funcionario in departamento.Funcionarios)
        {
            Console.WriteLine($"\tNome: {funcionario.Nome}");
        }
    }
}

static void EntendendoConsulta1NN1()
{
    using var db = new ApplicationContext();
    
    // Consulta 1 para N - LEFT JOIN
    var departamentos = db.Departamentos
        .Include(p=>p.Funcionarios)
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");
        foreach (var funcionario in departamento.Funcionarios)
        {
            Console.WriteLine($"\tNome: {funcionario.Nome}");
        }
    }

    // Consulta N para 1 - INNER JOIN
    var funcionarios = db.Funcionarios
        .Include(p => p.Departamento)
        .ToList();

    foreach (var funcionario in funcionarios)
    {
        Console.WriteLine($"Nome: {funcionario.Nome} / Descricap Dep: {funcionario.Departamento.Descricao}");
    }
}

static void ConsultaComTAG()
{
    //TAG: Utilizado para auditorias em banco de dados
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos
        .TagWith("""
                 Estou enviando um comentario para o servidor
                 Segundo comentario
                 Terceiro comentario
                 """)
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");
    }
}

static void ConsultaInterpolada()
{
    using var db = new ApplicationContext();

    var id = 1;
    var departamentos = db.Departamentos
        .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}")
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");
    }
}

static void ConsultaParametrizada()
{
    using var db = new ApplicationContext();

    var id = new SqlParameter
    {
        Value = 1,
        SqlDbType = SqlDbType.Int
    };
    var departamentos = db.Departamentos
        .FromSqlRaw("SELECT * FROM Departamentos WITH(NOLOCK) WHERE Id>{0}", id)
        .Where(p => !p.Excluido)
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");
    }
}

static void ConsultaProjetada()
{
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos
        .Where(p => p.Id > 0)
        .Select(p => new
        {
            p.Descricao, 
            Funcionarios = p.Funcionarios.Select(f => f.Nome) // com esse SELECT não precisa do INCLUDE na query
        })
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao}");

        foreach (var funcionario in departamento.Funcionarios)
        {
            Console.WriteLine($"\t Nome: {funcionario}");
        }
    }
}

static void IgnoreFiltroGlobal()
{
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos.IgnoreQueryFilters().Where(p => p.Id > 0).ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
    }
}

static void FiltroGlobal()
{
    //Aplicar HasQueryFilter no OnModelCreating no Context
    using var db = new ApplicationContext();

    var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
    }
}
#endregion

#region Setup

static void PopularDados()
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
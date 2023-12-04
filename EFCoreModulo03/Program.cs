//Modulo 3

using System.Diagnostics;
using EFCoreModulo03.Data;
using EFCoreModulo03.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

EnsureCreated();
EnsureDeleted();
GapDoEnsureCreated();
HealthCheckBancoDeDados();

// Quando True, reutiliza a conexão aberta
new ApplicationContext().Departamentos.AsNoTracking().Any(); //warmup
_count = 0;
GerenciarEstadoDaConexao(false);
_count = 0;
GerenciarEstadoDaConexao(true);

ExecuteSQL();
SqlInjection();
MigracoesPendentes();
AplicarMigracaoEmTempodeExecucao();
TodasMigracoes();
MigracoesJaAplicadas();
ScriptGeralDoBancoDeDados();

#region Modulo 3

static void ScriptGeralDoBancoDeDados()
{
    using var db = new ApplicationContext();
    var script = db.Database.GenerateCreateScript();

    Console.WriteLine(script);
}

static void MigracoesJaAplicadas()
{
    using var db = new ApplicationContext();

    var migracoes = db.Database.GetAppliedMigrations();

    Console.WriteLine($"Total: {migracoes.Count()}");

    foreach (var migracao in migracoes)
    {
        Console.WriteLine($"Migração: {migracao}");
    }
}

static void TodasMigracoes()
{
    using var db = new ApplicationContext();

    var migracoes = db.Database.GetMigrations();

    Console.WriteLine($"Total: {migracoes.Count()}");

    foreach (var migracao in migracoes)
    {
        Console.WriteLine($"Migração: {migracao}");
    }
}

static void AplicarMigracaoEmTempodeExecucao()
{
    using var db = new ApplicationContext();

    db.Database.EnsureDeleted();
    db.Database.Migrate();
}

static void MigracoesPendentes()
{
    using var db = new ApplicationContext();

    var migracoesPendentes = db.Database.GetPendingMigrations();

    Console.WriteLine($"Total: {migracoesPendentes.Count()}");

    foreach (var migracao in migracoesPendentes)
    {
        Console.WriteLine($"Migração: {migracao}");
    }
}

static void SqlInjection()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Departamentos.AddRange(
        new Departamento
        {
            Descricao = "Departamento 01"
        },
        new Departamento
        {
            Descricao = "Departamento 02"
        });
    db.SaveChanges();

    var descricao = "Teste ' or 1='1";
    
    //No método Raw, não deve ser usado interpolação de string
    db.Database.ExecuteSqlRaw("update departamentos set descricao='Departamento Alterado' where descricao={0}",descricao);
    //db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");
    foreach (var departamento in db.Departamentos.AsNoTracking())
    {
        Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
    }
}

static void ExecuteSQL()
{
    using var db = new ApplicationContext();
    const string descricao = "TESTE";

    // primeira opção
    using var cmd = db.Database.GetDbConnection().CreateCommand();
    cmd.Connection?.Open();
    cmd.CommandText = "SELECT 1";
    cmd.ExecuteNonQuery();
    cmd.Connection?.Close();

    // segunda opção
    db.Database.ExecuteSqlRaw("UPDATE DEPARTAMENTOS SET DESCRICAO={0} WHERE ID=1", descricao);
    
    // terceira opção
    db.Database.ExecuteSqlInterpolated($"UPDATE DEPARTAMENTOS SET DESCRICAO={descricao} WHERE ID=1");
}

static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
{
    using var db = new ApplicationContext();
    var time = Stopwatch.StartNew();

    var conexao = db.Database.GetDbConnection();
    conexao.StateChange += (sender, args) => ++_count; 

    if (gerenciarEstadoConexao)
    {
        conexao.Open();
    }
    
    for (var i = 0; i < 200; i++)
    {
        db.Departamentos.AsNoTracking().Any();
    }
    
    time.Stop();
    
    var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";
    Console.WriteLine(mensagem);
}

static void HealthCheckBancoDeDados()
{
    using var db = new ApplicationContext();

    var canConnect = db.Database.CanConnect();

    if (canConnect) {
        Console.WriteLine("Posso me conectar");
        return;
    }
    
    Console.WriteLine("Não posso me conectar");
}

static void GapDoEnsureCreated()
{
    using var db1 = new ApplicationContext();
    using var db2 = new ApplicationContextCidade();

    db1.Database.EnsureCreated();
    //db2.Database.EnsureCreated();

    // Quando há dois contextos é necessário forçar a criação da tabela do segundo contexto
    
    var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
    var temTabelas = databaseCreator.HasTables();
    
    if (!temTabelas) databaseCreator.CreateTables();
}

static void EnsureDeleted()
{
    using var db = new ApplicationContext();

    // dropa o database
    db.Database.EnsureDeleted();
}

static void EnsureCreated()
{
    using var db = new ApplicationContext();
    
    // cria o database e tabelas
    db.Database.EnsureCreated();
}

#endregion

public partial class Program
{
    static int _count = 0;
}
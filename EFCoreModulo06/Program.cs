using EFCoreModulo06.Data;
using EFCoreModulo06.Domain;
using EFCoreSetup;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
PopularDados();

//Modulo 6
CriarStoredProcedure();
InserirDadosViaProcedure();
CriarStoredProcedureDeConsulta();
ConsultaViaProcedure();


#region Modulo 6
static void ConsultaViaProcedure()
{
    using var db = new ApplicationContext();

    var dep = new SqlParameter("@dep", "Departamento");

    var departamentos = db.Departamentos
        .FromSqlRaw("EXECUTE GetDepartamentos @dep", dep)
        .ToList();

    foreach(var departamento in departamentos)
    {
        Console.WriteLine(departamento.Descricao);
    }
}

static void CriarStoredProcedureDeConsulta()
{
    var criarDepartamento = """
                            CREATE OR ALTER PROCEDURE GetDepartamentos
                                @Descricao VARCHAR(50)
                            AS
                            BEGIN
                                SELECT * FROM Departamentos Where Descricao Like @Descricao + '%'
                            END
                            """;
            
    using var db = new ApplicationContext();

    db.Database.ExecuteSqlRaw(criarDepartamento);
}

static void InserirDadosViaProcedure()
{
    using var db = new ApplicationContext();

    db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true);
}

static void CriarStoredProcedure()
{
    var criarDepartamento = """
                            CREATE OR ALTER PROCEDURE CriarDepartamento
                                @Descricao VARCHAR(50),
                                @Ativo bit
                            AS
                            BEGIN
                                INSERT INTO
                                    Departamentos(Descricao, Ativo, Excluido)
                                VALUES (@Descricao, @Ativo, 0)
                            END
                            """;
            
    using var db = new ApplicationContext();

    db.Database.ExecuteSqlRaw(criarDepartamento);
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
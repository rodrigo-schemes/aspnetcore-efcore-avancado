//Modulo 4

using EFCoreModulo04.Data;
using EFCoreModulo04.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
CarregamentoAdiantado();
CarregamentoExplicito();
CarregamentoLento();

#region Modulo 4

static void CarregamentoLento()
{
    //Instalar Microsoft.EntityFrameworkCore.Proxies
    //Habilitar no Context UseLazyLoadingProxies()
    //Desvantagens: Degradação da aplicação, mais queries e mais conexões abertas
    
    using var db = new ApplicationContext();
    SetupTiposCarregamentos(db);

    // Desabilita o LazyLoading
    //db.ChangeTracker.LazyLoadingEnabled = false;

    var departamentos = db
        .Departamentos
        .ToList();

    foreach (var departamento in departamentos)
    {
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Departamento: {departamento.Descricao}");

        if (departamento.Funcionarios?.Any() ?? false)
        {
            foreach (var funcionario in departamento.Funcionarios)
            {
                Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
            }
        }
        else
        {
            Console.WriteLine($"\tNenhum funcionario encontrado!");
        }
    }
}

static void CarregamentoExplicito()
{
    using var db = new ApplicationContext();
    SetupTiposCarregamentos(db);

    var departamentos = db
        .Departamentos
        .ToList();

    foreach (var departamento in departamentos)
    {
        if(departamento.Id == 2)
        {
            //db.Entry(departamento).Collection(p=>p.Funcionarios).Load();
            
            //caso seja necessário realizar filtros no carregamento
            db.Entry(departamento)
                .Collection(p=>p.Funcionarios)
                .Query()
                .Where(p=>p.Id > 2)
                .ToList();
        }

        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Departamento: {departamento.Descricao}");

        if (departamento.Funcionarios?.Any() ?? false)
        {
            foreach (var funcionario in departamento.Funcionarios)
            {
                Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
            }
        }
        else
        {
            Console.WriteLine($"\tNenhum funcionario encontrado!");
        }
    }
}

static void CarregamentoAdiantado()
{
    using var db = new ApplicationContext();
    SetupTiposCarregamentos(db);

    //Eager Loading - Traz os Departamentos e Funcionários em uma única consulta (LEFT JOIN)
    // Vantagens: A tabela principal deve ter poucas colunas
    // Desvantagens: Caso Departamentos tivesse muitos campos, Explosão cartesiana
    var departamentos = db
        .Departamentos
        .Include(p => p.Funcionarios);

    foreach (var departamento in departamentos)
    {

        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Departamento: {departamento.Descricao}");

        if (departamento.Funcionarios?.Any() ?? false)
        {
            foreach (var funcionario in departamento.Funcionarios)
            {
                Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
            }
        }
        else
        {
            Console.WriteLine("\tNenhum funcionario encontrado!");
        }
    }
}

static void SetupTiposCarregamentos(ApplicationContext db)
{
    if (!db.Departamentos.Any())
    {
        db.Departamentos.AddRange(
            new Departamento
            {
                Descricao = "Departamento 01",
                Funcionarios = new List<Funcionario>
                {
                    new Funcionario
                    {
                        Nome = "Rafael Almeida",
                        CPF = "99999999911",
                        RG= "2100062"
                    }
                }
            },
            new Departamento
            {
                Descricao = "Departamento 02",
                Funcionarios = new List<Funcionario>
                {
                    new Funcionario
                    {
                        Nome = "Bruno Brito",
                        CPF = "88888888811",
                        RG= "3100062"
                    },
                    new Funcionario
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
}

#endregion
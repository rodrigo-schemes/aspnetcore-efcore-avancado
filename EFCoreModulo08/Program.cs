using System.Text.Json;
using EFCoreModulo08.Data;
using EFCoreModulo08.Domain;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
Setup.GerarScripts(new ApplicationContext());
PopularRegistros();

//Modulo 8
ConversorCustomizado();
ShadowProperties();
OwnedTypes();
RelacionamentoUmParaUm();
RelacionamentoUmParaMuitos();
RelacionamentoMuitosParaMuitos();
BackingField();
HerancaTPH();
HerancaTPT();
PropertyBags();

#region Modulo 8

static void PropertyBags()
{
    using var db = new ApplicationContext();

    var configuracao = new Dictionary<string, object>
    {
        ["Chave"] = "SenhaBancoDeDados",
        ["Valor"] = Guid.NewGuid().ToString()
    };

    db.Configuracoes.Add(configuracao);
    db.SaveChanges();

    var configuracoes = db
        .Configuracoes
        .AsNoTracking()
        .Where(p => p["Chave"] == "SenhaBancoDeDados")
        .ToArray();

    foreach (var dic in configuracoes)
    {
        Console.WriteLine($"Chave: {dic["Chave"]} - Valor: {dic["Valor"]}");
    }
}

static void HerancaTPT()
{
    //Table Per Type - Herança
    using var db = new ApplicationContext();
    var loja = new Loja { Nome = "BW2" };

    var site = new Site { Nome = "Americanas", Url="www.americanas.com.br" };

    var fisica = new Fisica { Nome = "Ponto Frio", Endereco = "Shopping X"};

    db.AddRange(loja, site, fisica);
    db.SaveChanges();

    var lojas = db.Lojas.AsNoTracking().ToArray();
    var sites = db.Lojas.OfType<Site>().AsNoTracking().ToArray();
    var fisicas = db.Lojas.OfType<Fisica>().AsNoTracking().ToArray();

    Console.WriteLine("Lojas **************");
    foreach (var p in lojas)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}");
    }

    Console.WriteLine("Sites **************");
    foreach (var p in sites)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}, URL: {p.Url}");
    }

    Console.WriteLine("Fisicas **************");
    foreach (var p in fisicas)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Idade: {p.Endereco}");
    }
}

static void HerancaTPH()
{
    //Table Per Hierarchy - Herança
    using var db = new ApplicationContext();
    var pessoa = new Pessoa { Nome = "Fulano de Tal" };

    var instrutor = new Instrutor { Nome = "Rafael Almeida", Tecnologia = ".NET", Desde = DateTime.Now };

    var aluno = new Aluno { Nome = "Maria Thysbe", Idade = 31, DataContrato = DateTime.Now.AddDays(-1) };

    db.AddRange(pessoa, instrutor, aluno);
    db.SaveChanges();

    var pessoas = db.Pessoas.AsNoTracking().ToArray();
    var instrutores = db.Pessoas.OfType<Instrutor>().AsNoTracking().ToArray();
    var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();

    Console.WriteLine("Pessoas **************");
    foreach (var p in pessoas)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}");
    }

    Console.WriteLine("Instrutores **************");
    foreach (var p in instrutores)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Tecnologia: {p.Tecnologia}, Desde: {p.Desde}");
    }

    Console.WriteLine("Alunos **************");
    foreach (var p in alunos)
    {
        Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Idade: {p.Idade}, Data do Contrato: {p.DataContrato}");
    }
}

static void BackingField()
{
    using var db = new ApplicationContext();

    var documento = new Documento();
    documento.SetCPF("12345678933");

    db.Documentos.Add(documento);
    db.SaveChanges();

    foreach (var doc in db.Documentos.AsNoTracking())
    {
        Console.WriteLine($"CPF -> {doc.GetCPF()}");
    }
}

static void RelacionamentoMuitosParaMuitos()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    var ator1 = new Ator { Nome = "Rafael" };
    var ator2 = new Ator { Nome = "Pires" };
    var ator3 = new Ator { Nome = "Bruno" };

    var filme1 = new Filme { Descricao = "A volta dos que não foram" };
    var filme2 = new Filme { Descricao = "De volta para o futuro" };
    var filme3 = new Filme { Descricao = "Poeira em alto mar filme" };

    ator1.Filmes.Add(filme1);
    ator1.Filmes.Add(filme2);

    ator2.Filmes.Add(filme1);

    filme3.Atores.Add(ator1);
    filme3.Atores.Add(ator2);
    filme3.Atores.Add(ator3);

    db.AddRange(ator1, ator2, filme3);

    db.SaveChanges();

    foreach (var ator in db.Atores.Include(e => e.Filmes))
    {
        Console.WriteLine($"Ator: {ator.Nome}");

        foreach (var filme in ator.Filmes)
        {
            Console.WriteLine($"\tFilme: {filme.Descricao}");
        }
    }
}

static void RelacionamentoUmParaMuitos()
{
    using (var db = new ApplicationContext())
    {

        var estado = new Estado
        {
            Nome = "Sergipe",
            Governador = new Governador { Nome = "Rafael Almeida", Partido = "PT"}
        };

        estado.Cidades.Add(new Cidade { Nome = "Itabaiana" });

        db.Estados.Add(estado);

        db.SaveChanges();
    }

    using (var db = new ApplicationContext())
    {
        var estados = db.Estados.ToList();

        estados[0].Cidades.Add(new Cidade { Nome = "Aracaju" });

        db.SaveChanges();

        foreach (var est in db.Estados.Include(p => p.Cidades).AsNoTracking())
        {
            Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");

            foreach (var cidade in est.Cidades)
            {
                Console.WriteLine($"\t Cidade: {cidade.Nome}");
            }
        }
    }
}

static void RelacionamentoUmParaUm()
{
    using var db = new ApplicationContext();

    var estado = new Estado
    {
        Nome = "Minas Gerais",
        Governador = new Governador
        {
            Nome = "Zema",
            Partido = "Novo"
        }
    };

    db.Estados.Add(estado);
    db.SaveChanges();

    var estados = db.Estados
        .AsNoTracking()
        .ToList();
    
    estados.ForEach(est =>
    {
        Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
    });
}

static void OwnedTypes()
{
    // Uma entidade pertcence a outra entidade pai
    // Pode ser gravado na mesma tabela no DB ou em tabelas separadas
    using var db = new ApplicationContext();

    var cliente = new Cliente
    {
        Nome = "Fulano de tal",
        Telefone = "(79) 98888-9999",
        Endereco = new Endereco { Bairro = "Centro", Cidade = "Sao Paulo", Estado = "SP", Logradouro = "Rua X"}
    };

    db.Clientes.Add(cliente);

    db.SaveChanges();

    var clientes = db.Clientes.AsNoTracking().ToList();

    var options = new JsonSerializerOptions { WriteIndented = true };

    clientes.ForEach(cli =>
    {
        var json = JsonSerializer.Serialize(cli, options);

        Console.WriteLine(json);
    });
}

static void ShadowProperties()
{
    using var db = new ApplicationContext();

    var departamento = new Departamento
    {
        Descricao = "Departamento Propriedade de Sombra"
    };

    db.Departamentos.Add(departamento);

    db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

    db.SaveChanges();
    
    db.Departamentos.Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
}

static void ConversorCustomizado()
{
    using var db = new ApplicationContext();

    db.Conversores.Add(
        new Conversor
        {
            Status = Status.Devolvido,
        });

    db.SaveChanges();

    // Irá consultar pela letra 'A'
    db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);
    
    // Irá consultar pela letra 'D'
    db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
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



using System.Transactions;
using EFCoreModulo12.Data;
using EFCoreModulo12.Domain;
using Microsoft.EntityFrameworkCore;

//Modulo 12
ComportamentoPadrao();
GerenciandoTransacaoManualmente();
ReverterTransacao();
SalvarPontoTransacao();
TransactionScope();

#region Modulo 12
static void TransactionScope()
{
    //TransactionScope: Classe que faz transações entre métodos
    CadastrarLivro();

    // Alterar o nível de isolamento da transação
    var transactionOptions = new TransactionOptions
    {
        IsolationLevel = IsolationLevel.ReadCommitted
    };

    using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions);
    ConsultarAtualizar();
    CadastraLivroEnterprise();
    CadastrarLivroDominandoEFCore();

    scope.Complete();
} 

static void ConsultarAtualizar()
{
    using var db = new ApplicationContext();
    var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
    livro.Autor = "Rafael Almeida"; 
    db.SaveChanges();
}

static void CadastraLivroEnterprise()
{
    using var db = new ApplicationContext();
    db.Livros.Add(
        new Livro
        {
            Titulo = "ASP.NET Core Enterprise Applications",
            Autor = "Eduardo Pires"
        });  
    db.SaveChanges();
}

static void CadastrarLivroDominandoEFCore()
{
    using var db = new ApplicationContext();
    db.Livros.Add(
        new Livro
        {
            Titulo = "Dominando o Entity Framework Core",
            Autor = "Rafael Almeida"
        });  
    db.SaveChanges();
}

static void SalvarPontoTransacao()
{
    CadastrarLivro();

    using var db = new ApplicationContext();
    using var transacao = db.Database.BeginTransaction();

    try
    {
        var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
        livro.Autor = "Rafael Almeida"; 
        db.SaveChanges();

        // em caso de falha, até este ponto não será desfeito
        transacao.CreateSavepoint("desfazer_apenas_insercao");

        db.Livros.Add(
            new Livro
            {
                Titulo = "ASP.NET Core Enterprise Applications",
                Autor = "Eduardo Pires"
            });  
        db.SaveChanges();    

        db.Livros.Add(
            new Livro
            {
                Titulo = "Dominando o Entity Framework Core",
                Autor = "Rafael Almeida".PadLeft(16,'*')
            });  
        db.SaveChanges();     

        transacao.Commit();            
    }
    catch(DbUpdateException e)
    {
        transacao.RollbackToSavepoint("desfazer_apenas_insercao");

        if(e.Entries.Count(p=>p.State == EntityState.Added) == e.Entries.Count)
        {
            transacao.Commit();
        }
    }
} 

static void ReverterTransacao()
{
    CadastrarLivro();

    using var db = new ApplicationContext();
    var transacao = db.Database.BeginTransaction();

    try
    {
        var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
        livro.Autor = "Rafael Almeida"; 
        db.SaveChanges();

        db.Livros.Add(
            new Livro
            {
                Titulo = "Dominando o Entity Framework Core",
                Autor = "Rafael Almeida".PadLeft(16,'*')
            }); 

        db.SaveChanges();     

        transacao.Commit();            
    }
    catch(Exception)
    {
        transacao.Rollback();
    }
}

static void GerenciandoTransacaoManualmente()
{
    CadastrarLivro();

    using var db = new ApplicationContext();
    var transacao = db.Database.BeginTransaction();

    var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
    livro.Autor = "Rafael Almeida";

    db.SaveChanges();

    db.Livros.Add(
        new Livro
        {
            Titulo = "Phojeto Phoenix",
            Autor = "Gene Simons"
        }); 

    db.SaveChanges();     
    transacao.Commit();
}

static void ComportamentoPadrao()
{
    // comportamento padrão: Todas as mudanças do contexto serão salvas
    CadastrarLivro();

    using var db = new ApplicationContext();
    var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
    livro.Autor = "Rafael Almeida"; 

    db.Livros.Add(
        new Livro
        {
            Titulo = "Dominando o Entity Framework Core",
            Autor = "Rafael Almeida"
        }); 

    db.SaveChanges();
}
#endregion

#region Setup

static void CadastrarLivro()
{
    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Livros.Add(
        new Livro
        {
            Titulo = "Introdução ao Entity Framework Core",
            Autor = "Rafael"
        }); 

    db.SaveChanges();
}

#endregion
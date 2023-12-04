using EFCoreModulo16.Data;
using EFCoreModulo16.Domain;

ListarPessoas(new ApplicationContext(EnumDatabase.SQLSERVER));
ListarPessoas(new ApplicationContext(EnumDatabase.POSTGRES));
ListarPessoas(new ApplicationContext(EnumDatabase.SQLITE));
ListarPessoas(new ApplicationContext(EnumDatabase.INMEMORY));
ListarPessoas(new ApplicationContext(EnumDatabase.COSMOSDB));
return;

static void ListarPessoas(ApplicationContext db)
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated(); 
            
    db.Pessoas.Add(new Pessoa
    {
        Id=Guid.NewGuid(),
        Nome = "TESTE",
        Telefone = "999999"
    });

    db.SaveChanges();
            
    var pessoas = db.Pessoas.ToList();
    foreach(var item in pessoas)
    {
        Console.WriteLine($"Nome: {item.Nome}");
    }
}


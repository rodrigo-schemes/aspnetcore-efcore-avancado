using EFCoreModulo09.Data;
using EFCoreModulo09.Domain;
using EFCoreSetup;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());
Setup.GerarScripts(new ApplicationContext());

// Modulo 9
Atributos();

#region Modulo 9

static void Atributos()
{
    using var db = new ApplicationContext();

    db.Atributos.Add(new Atributo
    {
        Descricao = "Exemplo",
        Observacao = "Observacao"
    });

    db.SaveChanges();
}

#endregion

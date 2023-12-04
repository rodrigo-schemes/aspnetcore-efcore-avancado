using EFCoreModulo21.Data;
using EFCoreSetup;
using Microsoft.EntityFrameworkCore;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());

GeradorDeSql();
return;


static void GeradorDeSql()
{
    using var db = new ApplicationContext();
    var sql = db.Departamentos.Where(p => p.Id > 0).ToQueryString();
        
    Console.WriteLine(sql);
}
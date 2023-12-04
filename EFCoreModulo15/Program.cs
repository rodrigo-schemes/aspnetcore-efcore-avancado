using EFCoreModulo15.Data;
using Microsoft.EntityFrameworkCore;

using var db = new ApplicationContext();

//db.Database.Migrate();

var migracoes = db.Database.GetPendingMigrations();
foreach(var migracao in migracoes)
{
    Console.WriteLine(migracao);
}
            
Console.WriteLine("Hello World!");
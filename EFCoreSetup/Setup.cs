using Microsoft.EntityFrameworkCore;

namespace EFCoreSetup;

public static class Setup
{
    public static string GetConnectionString()
    {
        return
            "Server=localhost,1433;Database=CursoEFCoreAvancado;User Id=sa;Password=P@ssword!;TrustServerCertificate=true";
    }
    
    public static void CriarDatabaseParaExemplos(DbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        db.Dispose();
    }

    public static void GerarScripts(DbContext db)
    {
        var script = db.Database.GenerateCreateScript();

        Console.WriteLine(script);
        
        db.Dispose();
    }
}
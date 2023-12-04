using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModulo11.Interceptadores;

public class InterceptadorPersistencia : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
            
        Console.WriteLine("[Save] Entrei no metodo SaveChanges");
        Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);

        return result;
    }
}
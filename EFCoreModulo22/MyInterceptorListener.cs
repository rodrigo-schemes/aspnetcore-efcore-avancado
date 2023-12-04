using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo22;

public class MyInterceptorListener : IObserver<DiagnosticListener>
{
    private readonly MyInterceptor _interceptor = new();
                
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(DiagnosticListener listener)
    {
        if(listener.Name == DbLoggerCategory.Name)
        {
            listener.Subscribe(_interceptor);
        }
    }
}
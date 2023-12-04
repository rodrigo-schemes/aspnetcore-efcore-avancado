using System.Diagnostics;
using EFCoreModulo22;
using EFCoreModulo22.Data;
using EFCoreSetup;

Setup.CriarDatabaseParaExemplos(new ApplicationContext());

Diagnostics();
return;

static void Diagnostics()
{
    DiagnosticListener.AllListeners.Subscribe(new MyInterceptorListener());
    using var db = new ApplicationContext();
    db.Departamentos.Where(p => p.Id > 0).ToArray();
}
using EFCoreModulo18.Data.Repositories;

namespace EFCoreModulo18.Data
{
    public interface IUnitOfWork : IDisposable
    {
         bool Commit();
         IDepartamentoRepository DepartamentoRepository{get;}
    }
}
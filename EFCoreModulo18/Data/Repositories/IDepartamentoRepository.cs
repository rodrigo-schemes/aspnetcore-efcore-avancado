using EFCoreModulo18.Data.Repositories.Base;
using EFCoreModulo18.Domain;

namespace EFCoreModulo18.Data.Repositories
{
    public interface IDepartamentoRepository : IGenericRepository<Departamento>
    {
         //Task<Departamento> GetByIdAsync(int id); 
         //void Add(Departamento departamento);
         //bool Save();
    }
}
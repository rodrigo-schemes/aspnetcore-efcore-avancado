using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCoreModulo17.Schema.Data.ModelFactory;

//Usado na Estratégia 2 - Aplica o schema no DbContext
public class StrategySchemaModelCacheKey : IModelCacheKeyFactory
{ 
    public object Create(DbContext context, bool designTime)
    {
        var model = new 
        {
            Type = context.GetType(),
            Schema =  (context as ApplicationContext)?.TenantData.TenantId
        };

        return (model, designTime);
    }
}
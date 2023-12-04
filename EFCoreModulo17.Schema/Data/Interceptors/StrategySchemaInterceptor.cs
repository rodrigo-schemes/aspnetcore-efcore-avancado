using System.Data;
using System.Data.Common;
using EFCoreModulo17.Schema.Provider;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModulo17.Schema.Data.Interceptors
{
    //Usado na Estratégia 2 - Faz o replace do schema
    public class StrategySchemaInterceptor(TenantData tenantData) : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, 
            CommandEventData eventData, 
            InterceptionResult<DbDataReader> result)
        {
            ReplaceSchema(command);
            
            return base.ReaderExecuting(command, eventData, result);
        }

        private void ReplaceSchema(IDbCommand command)
        {
            // FROM PRODUCTS -> FROM [tenant-1].PRODUCTS
            command.CommandText = command.CommandText
                .Replace("FROM ", $" FROM [{tenantData.TenantId}].")
                .Replace("JOIN ", $" JOIN [{tenantData.TenantId}].");
        }
    }
}
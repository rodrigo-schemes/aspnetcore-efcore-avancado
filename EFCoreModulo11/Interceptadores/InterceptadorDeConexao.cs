using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModulo11.Interceptadores;

public class InterceptadorDeConexao : DbConnectionInterceptor
{
    public override InterceptionResult ConnectionOpening(
        DbConnection connection, 
        ConnectionEventData eventData, 
        InterceptionResult result)
    {
        Console.WriteLine("[ConnOpen] Entrei no metodo ConnectionOpening");

        var connectionString = ((SqlConnection)connection).ConnectionString;

        Console.WriteLine(connectionString);

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            //DataSource="IP Segundo Servidor",
            ApplicationName = "CursoEFCore"
        };

        Console.WriteLine(connectionStringBuilder.ToString());

        return result;
    }
}
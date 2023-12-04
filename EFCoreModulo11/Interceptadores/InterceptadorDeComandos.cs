using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModulo11.Interceptadores;

public partial class InterceptadorDeComandos : DbCommandInterceptor
{
    private static readonly Regex TableRegex =
        MyRegex();

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine("[Sync] Entrei no interceptador de Comandos");
        UsarNoLock(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result, 
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[Async] Entrei no interceptador de Comandos");
        UsarNoLock(command);

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void UsarNoLock(DbCommand command)
    {
        if (!command.CommandText.Contains("WITH (NOLOCK)")
            && command.CommandText.StartsWith("-- Use NOLOCK"))
        {
            command.CommandText = TableRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
        }
    }

    [GeneratedRegex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", 
        RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "pt-BR")]
    private static partial Regex MyRegex();
}
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreModulo22;

public class MyInterceptor : IObserver<KeyValuePair<string, object>>
{
    private static readonly Regex _tableAliasRegex =
        new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
            RegexOptions.Multiline | 
            RegexOptions.IgnoreCase | 
            RegexOptions.Compiled);
                
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        if(value.Key == RelationalEventId.CommandExecuting.Name)
        {
            var command = ((CommandEventData)value.Value).Command;

            if(!command.CommandText.Contains("WITH (NOLOCK)"))
            {
                command.CommandText = _tableAliasRegex.Replace(command.CommandText,"${tableAlias} WITH (NOLOCK)" );

                Console.WriteLine(command.CommandText);
            }
        }
    }
}
using EFCoreModulo08.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreModulo08.Conversores;

public class ConversorCustomizado() : ValueConverter<Status, string>(p=> ConverterParaBancoDeDados(p),
    value => ConverterParaAplicacao(value),
    new ConverterMappingHints(1)) // Define o tamanho do campo no DB
{
    private static string ConverterParaBancoDeDados(Status status)
    {
        // Grava a primeira letra do ENUM no DB
        return status.ToString()[0..1];
    }

    private static Status ConverterParaAplicacao(string value)
    {
        // Converte o registro do DB para o Enum
        var status = Enum
            .GetValues<Status>()
            .FirstOrDefault(p=>p.ToString()[0..1] == value);

        return status;
    }
}
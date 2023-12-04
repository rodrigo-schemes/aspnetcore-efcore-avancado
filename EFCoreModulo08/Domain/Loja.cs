namespace EFCoreModulo08.Domain;

public class Loja
{
    public int Id { get; set; }
    public string Nome { get; set; } 
}

public class Site : Loja
{
    public string Url { get; set; } 
}

public class Fisica : Loja
{
    public string Endereco { get; set; } 
}
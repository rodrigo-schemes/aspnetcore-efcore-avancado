namespace EFCoreModulo04.Domain;

public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string RG { get; set; }

    public int DeparmentoId { get; set; }
    public virtual Departamento Departamento { get; set; }
}
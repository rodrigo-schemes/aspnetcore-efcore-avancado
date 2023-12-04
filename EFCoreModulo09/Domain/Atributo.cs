using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModulo09.Domain;

[Table("TabelaAtributos")]
[Index(nameof(Descricao), nameof(Id), IsUnique = true)]
[Comment("Meu comentario de minha tabela")]
public class Atributo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Comment("Meu comentario para meu campo")]
    [Column("MinhaDescricao", TypeName = "VARCHAR(100)")]
    [Required]
    public string Descricao { get; set; } 

    
    [MaxLength(255)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? Observacao { get; set; } 
}

public class Aeroporto
{
    public int Id { get; set; }
    public string Nome { get; set; }
        
    [InverseProperty("AeroportoPartida")]
    public ICollection<Voo> VoosDePartida { get; set; }

    [InverseProperty("AeroportoChegada")]
    public ICollection<Voo> VoosDeChegada { get; set; }
    
    [NotMapped]
    public string PropriedadeTeste { get; set; }
}

[NotMapped]
public class Voo
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public Aeroporto? AeroportoPartida { get; set; }
    public Aeroporto? AeroportoChegada { get; set; }
}

[Keyless]
public class RelatorioFinanceiro
{
    public string Descricao { get; set; } 
    public decimal Total { get; set; } 
    public DateTime Data { get; set; } 
}

using EFCoreModulo17.BancoDeDados.Domain.Abstract;

namespace EFCoreModulo17.BancoDeDados.Domain
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
    }
}
using EFCoreModulo17.Schema.Domain.Abstract;

namespace EFCoreModulo17.Schema.Domain
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
    }
}
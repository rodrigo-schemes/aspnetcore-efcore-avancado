using EFCoreModulo17.Tabela.Data;
using EFCoreModulo17.Tabela.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.Tabela.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> Get([FromServices]ApplicationContext db)
        {
           var products = db.Products.ToArray();

           return products;
        }
    }
}

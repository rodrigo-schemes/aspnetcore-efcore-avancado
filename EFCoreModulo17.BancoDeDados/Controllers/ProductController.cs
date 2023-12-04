using EFCoreModulo17.BancoDeDados.Data;
using EFCoreModulo17.BancoDeDados.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.BancoDeDados.Controllers
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

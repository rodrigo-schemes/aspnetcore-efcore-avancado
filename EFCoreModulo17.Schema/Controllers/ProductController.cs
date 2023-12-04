using EFCoreModulo17.Schema.Data;
using EFCoreModulo17.Schema.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.Schema.Controllers
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

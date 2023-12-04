using EFCoreModulo17.Tabela.Data;
using EFCoreModulo17.Tabela.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.Tabela.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Person> Get([FromServices]ApplicationContext db)
        {
           var people = db.People.ToArray();

           return people;
        }
    }
}

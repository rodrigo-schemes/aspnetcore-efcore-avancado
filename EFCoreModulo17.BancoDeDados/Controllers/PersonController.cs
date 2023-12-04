using EFCoreModulo17.BancoDeDados.Data;
using EFCoreModulo17.BancoDeDados.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.BancoDeDados.Controllers
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

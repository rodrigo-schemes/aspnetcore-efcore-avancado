using EFCoreModulo17.Schema.Data;
using EFCoreModulo17.Schema.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreModulo17.Schema.Controllers
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

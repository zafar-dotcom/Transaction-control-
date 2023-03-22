using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmailViaSMTP.DAL_Services;

namespace SendEmailViaSMTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionScopeController : ControllerBase
    {
        [Route("transaction")]
        public IActionResult Transaction()
        {
            DAL obj = new DAL();
            if (obj.TransactionScope())
            {
                obj.BeginTransaction();
                return Ok();

            }
            
            else
            return BadRequest();
        }
    }
}

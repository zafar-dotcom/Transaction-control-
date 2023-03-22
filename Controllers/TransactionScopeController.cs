using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmailViaSMTP.DAL_Services;
using SendEmailViaSMTP.Models;

namespace SendEmailViaSMTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionScopeController : ControllerBase
    {
        [Route("transaction")]
        [HttpPost]
        public IActionResult Transaction([FromBody] TransactionModel modl)
        {
            DAL obj = new DAL();
            if (obj.TransactionScope(modl))
            {
              //  obj.BeginTransaction();
                return Ok();

            }
            
            else
            return BadRequest();
        }
    }
}

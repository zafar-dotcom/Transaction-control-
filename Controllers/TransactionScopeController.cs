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
        public DAL dal;
        public TransactionScopeController()
        {
            dal = new DAL();

        }
        [Route("transaction")]
        [HttpPost]
        public IActionResult Transaction([FromBody] TransactionModel modl)
        {
            
            if (dal.TransactionScope(modl))
            {
              dal.BeginTransaction(modl);
                return Ok();

            }
            
            else
            return BadRequest();
        }
        [Route("Get_Applicant_experience")]
        [HttpGet]
        public IActionResult ApplicantExperience()
        {
           var data= dal.GetfromMasterDEtail();
            return Ok(data);
        }
        [Route("update_Applicant_experience")]
        [HttpPut]
        public IActionResult Update_master_detail(TransactionModel modl)
        {
            if (dal.Update_master_detail(modl))
            {
                return Ok();

            }
            else
                return BadRequest();
        }

        [Route("delete_Applicant_experience")]
        [HttpDelete]
        public IActionResult Delete_master_detail(int id)
        {
            if (dal.Delete_master_Detail(id))
            {
                return Ok();

            }
            else
                return BadRequest();
        }
    }
}

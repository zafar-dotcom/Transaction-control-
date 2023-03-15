using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmailViaSMTP.DAL_Services;
using SendEmailViaSMTP.Models;

namespace SendEmailViaSMTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitTestController : ControllerBase
    {
       DAL dal=new DAL();
        [Route("listofemployee")]
        public List<UserModel> GetUser()
        {
            var list = dal.GetEmployee();
            return list;
        }
        [Route("insert")]
        [HttpPost]
        public IActionResult Insert(UserModel user)
        {

            if (dal.Add_user(user))
            {
                return Ok();
            }
            else
                return BadRequest("User not inserted");
        }

        [Route("update")]
        [HttpPut]
        public IActionResult Update(UserModel user)
        {
            if (dal.Update(user))
            {
                return Ok();
            }
            else
                return BadRequest("Not updated");
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {

            if (dal.Delete(id))
            {
                return Ok();
            }
            else
                return BadRequest("Not Deleted");
        }

    }
}

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
    }
}

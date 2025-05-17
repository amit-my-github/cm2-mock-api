using Microsoft.AspNetCore.Mvc;
using Content.Manager.Core.WebApi.Model;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<UserItem?> Get()
        {
            return Services.UserServiceProvider.GetAll();
        }
        
    }
}

using Asp.Versioning;
using CM.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<UserItem?> Get()
        {
            return Services.UserServiceProvider.GetAll();
        }

    }
}

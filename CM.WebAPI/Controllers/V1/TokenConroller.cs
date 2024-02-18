using Asp.Versioning;
using CM.WebAPI.Model.Token;
using Microsoft.AspNetCore.Mvc;

namespace CM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/token")]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public ActionResult<TokenItem?> Get() => Services.TokenServiceProvider.GetAll();

    }
}

using Content.Manager.Core.WebApi.Model.Token;
using Microsoft.AspNetCore.Mvc;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public ActionResult<TokenItem?> Get() => Services.TokenServiceProvider.GetAll();

    }
}

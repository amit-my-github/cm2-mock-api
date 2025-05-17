using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Series;
using Microsoft.AspNetCore.Mvc;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<SeriesResponse> Get([FromQuery] SeriesQueryParameters parameters)
        {
            return Services.SeriesServiceProvider.GetAll(parameters);
        }

    }
}

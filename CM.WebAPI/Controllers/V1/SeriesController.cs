using Asp.Versioning;
using CM.WebAPI.Model.Parameters;
using CM.WebAPI.Model.Series;
using Microsoft.AspNetCore.Mvc;

namespace CM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/series")]
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

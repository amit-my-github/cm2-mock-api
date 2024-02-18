using Asp.Versioning;
using CM.WebAPI.Model.Parameters;
using CM.WebAPI.Model.Publishers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/publishers")]
    public class PublishersController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<PublishersResponse> Get([FromQuery] PublisherQueryParameters parameters)
        {
            return Services.PublisherServiceProvider.GetAll(parameters);
        }

        [Route("{id:int}")]
        [HttpGet]
        public ActionResult<PublisherItem> Get(int id)
        {
            var publisher = Services.PublisherServiceProvider.GetById(id);
            if (publisher != null)
            {
                return publisher;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddPublisherRequest publisher)
        {

            var res = Services.PublisherServiceProvider.Create(publisher);
            string? msg;
            if (res > 0)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publisher has been successfully added!" });
            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] AddPublisherRequest publisher)
        {
            string? msg;
            if (!Services.PublisherServiceProvider.IsValidRequest(id))
            {
                return NotFound("Not Found");
            }
            var res = Services.PublisherServiceProvider.Update(id, publisher);
            if (res)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publisher has been updated successfully!" });

            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var accessCode = Services.PublisherServiceProvider.GetById(id);
            //if (accessCode == null)
            //{
            //    return NotFound();
            //}
            //var res = Services.PublisherServiceProvider.Remove(accessCode);
            //if (res)
            //{
            //    var msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publisher has been deleted successfully!" });
            //    return Ok(msg);
            //}
            //return NoContent();
            string? msg;
            if (!Services.PublisherServiceProvider.IsValidRequest(id))
            {
                return NotFound("Not Found");
            }
            var res = Services.PublisherServiceProvider.Remove(id);
            if (res)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publisher has been deleted successfully!" });

            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);
        }

    }

}

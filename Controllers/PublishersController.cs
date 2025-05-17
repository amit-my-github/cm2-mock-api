using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Publishers;
using Content.Manager.Core.WebApi.Model.Publishers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/publishers")]
    [ApiController]
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
        public IActionResult Create([FromBody] PublisherItem publisher)
        {
           
            var res = Services.PublisherServiceProvider.Create(publisher);
            string? msg;
            if (res > 0)
            {
                msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publisher has been successfully added!" });
            }
            else
            {
                msg = JsonConvert.SerializeObject(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] PublisherItem publisher)
        {
            if (id != publisher.Id)
                return BadRequest();
            var objPublisher = Services.PublisherServiceProvider.GetById(id);
            if (objPublisher == null)
            {
                return NotFound();
            }
            var res = Services.PublisherServiceProvider.Update(id, publisher);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publisher has been updated successfully!" });
                return Ok(msg);
            }
            return NoContent();

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var accessCode = Services.PublisherServiceProvider.GetById(id);
            if (accessCode == null)
            {
                return NotFound();
            }
            var res = Services.PublisherServiceProvider.Remove(accessCode);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publisher has been deleted successfully!" });
                return Ok(msg);
            }
            return NoContent();
        }

    }


}

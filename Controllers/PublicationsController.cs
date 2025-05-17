using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Publication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/publicationtitles")]
    [ApiController]
    public class PublicationTitlesController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<PublicationTitlesOverViewResponse> Get([FromQuery] PublicationQueryParameters parameters)
        {
            return Services.PublicationServiceProvider.GetAll(parameters);
        }


        [Route("{id:int}")]
        [HttpGet]
        public ActionResult<PublicationTitleItem> Get(int id)
        {
            var publisher = Services.PublicationServiceProvider.GetById(id);
            if (publisher != null)
            {
                return publisher;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] PublicationTitleItem publicationTitles)
        {

            var res = Services.PublicationServiceProvider.Create(publicationTitles);
            string? msg;
            if (res > 0)
            {
                msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publication Title has been successfully added!" });
            }
            else
            {
                msg = JsonConvert.SerializeObject(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] PublicationTitleItem publicationTitle)
        {
            if (id != publicationTitle.Id)
                return BadRequest();
            if (!Services.PublicationServiceProvider.IsValidRequest(id))
            {
                return NotFound();
            }
            var res = Services.PublicationServiceProvider.Update(id, publicationTitle);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publication Title has been updated successfully!" });
                return Ok(msg);
            }
            return NoContent();

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisherId = Services.PublicationServiceProvider.GetPublicationTitleId(id);
            if (!Services.PublicationServiceProvider.IsValidRequest(id))
            {
                return NotFound();
            }
            var res = Services.PublicationServiceProvider.Remove(publisherId);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Publication Title has been deleted successfully!" });
                return Ok(msg);
            }
            return NoContent();
        }



    }


}

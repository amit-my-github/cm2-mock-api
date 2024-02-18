using Asp.Versioning;
using CM.WebAPI.Model.Parameters;
using CM.WebAPI.Model.Publication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CM.WebAPI.Controllers.V1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/publicationtitles")]
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

        [Route("{id:int}/mapping-relations-details")]
        [HttpGet]
        public ActionResult<PublicationTitleMappingAndRelationResponse> GetMappingAndRelations(int id)
        {
            var publisher = Services.PublicationServiceProvider.GetMappingAndRelations(id);
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
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publication Title has been successfully added!" });
            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
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
                var msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publication Title has been updated successfully!" });
                return Ok(msg);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Services.PublicationServiceProvider.IsValidRequest(id))
            {
                return NotFound();
            }
            var res = Services.PublicationServiceProvider.Remove(id);
            if (res)
            {
                var msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Publication Title has been deleted successfully!" });
                return Ok(msg);
            }
            return NoContent();
        }


    }

}

using Content.Manager.Core.WebApi.Model.AccessCodes;
using Content.Manager.Core.WebApi.Model.Parameters;
using Content.Manager.Core.WebApi.Model.Publication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/access-codes")]
    [ApiController]
    public class AccessCodesController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<AccessCodeResponse> Get([FromQuery] AccessCodeQueryParameters parameters)
        {
            return Services.AccessCodeServiceProvider.GetAll(parameters);
        }


        [Route("{id:int}")]
        [HttpGet]
        public ActionResult<AccessCodeItem> Get(int id)
        {
            var accessCode = Services.AccessCodeServiceProvider.GetById(id);
            if (accessCode != null)
            {
                return accessCode;
            }
            return NotFound();
        }

        [Route("{code}")]
        [HttpGet]
        public ActionResult<AccessCodeItem> Get(string code)
        {
            var accessCode = Services.AccessCodeServiceProvider.GetByCode(code);
            if (accessCode != null)
            {
                return accessCode;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] AccessCodeItem accessCode)
        {
            var res = Services.AccessCodeServiceProvider.Create(accessCode);
            string? msg;
            if (res > 0)
            {
                msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Access code has been successfully added!" });
            }
            else
            {
                msg = JsonConvert.SerializeObject(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] AccessCodeItem accessCode)
        {
            if (id != accessCode.Id)
                return BadRequest();
            if (!Services.AccessCodeServiceProvider.IsValidRequest(id))
            {
                return NotFound();
            }
            var res = Services.AccessCodeServiceProvider.Update(id, accessCode);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Access code has been updated successfully!" });
                return Ok(msg);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Services.AccessCodeServiceProvider.IsValidRequest(id))
            {
                return NotFound();
            }
            var res = Services.AccessCodeServiceProvider.Remove(id);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Access code has been deleted successfully!" });
                return Ok(msg);
            }
            return NoContent();
        }

        [Route("{accessCode}/publicationtitles")]
        [HttpGet]
        public ActionResult<PublicationTitlesResponse> GetPublicationTitlesByAccessCode(string accessCode, [FromQuery] PublicationQueryParameters parameters)
        {
            var publicationTitles = Services.PublicationServiceProvider.GetPublicationTitlesByAccessCode(accessCode, parameters);
            if (publicationTitles != null)
            {
                return Ok(publicationTitles);
            }
            return NoContent();
        }


    }
}

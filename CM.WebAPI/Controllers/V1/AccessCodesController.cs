using CM.WebAPI.Model.AccessCodes;
using CM.WebAPI.Model.Parameters;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace CM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/access-codes")]
    public class AccessCodesController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public IActionResult Get([FromQuery] AccessCodeQueryParameters parameters)
        {
            var accessCodes = Services.AccessCodeServiceProvider.GetAll(parameters);
            return Ok(accessCodes);

        }

        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {

            var accessCode = Services.AccessCodeServiceProvider.GetById(id);
            return accessCode == null ? NotFound("Not Found") : Ok(accessCode);
        }

        [Route("{code}")]
        [HttpGet]
        public IActionResult Get(string code)
        {
            var accessCode = Services.AccessCodeServiceProvider.GetByCode(code); ;
            return accessCode == null ? NotFound("Not Found") : Ok(accessCode);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddAccessCodeRequest accessCode)
        {
            string? msg;
            var res = Services.AccessCodeServiceProvider.Create(accessCode);
            if (res > 0)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", message = "Access code has been successfully added!" });
            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] AddAccessCodeRequest accessCode)
        {
            string? msg;
            if (!Services.AccessCodeServiceProvider.IsValidRequest(id))
            {
                return NotFound("Not Found");
            }
            var res = Services.AccessCodeServiceProvider.Update(id, accessCode);
            if (res)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Access code has been updated successfully!" });

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
            string? msg;
            if (!Services.AccessCodeServiceProvider.IsValidRequest(id))
            {
                return NotFound("Not Found");
            }
            var res = Services.AccessCodeServiceProvider.Remove(id);
            if (res)
            {
                msg = JsonSerializer.Serialize(new { status = "SUCCESS", Message = "Access code has been deleted successfully!" });

            }
            else
            {
                msg = JsonSerializer.Serialize(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);
        }

        [Route("{accessCode}/publicationtitles")]
        [HttpGet]
        public IActionResult GetPublicationTitlesByAccessCode(string accessCode, [FromQuery] PublicationQueryParameters parameters)
        {

            var publicationTitles = Services.PublicationServiceProvider.GetPublicationTitlesByAccessCode(accessCode, parameters); ;
            return Ok(publicationTitles);
        }

    }
}

using Content.Manager.Core.WebApi.Model.Category;
using Content.Manager.Core.WebApi.Model.Facet;
using Content.Manager.Core.WebApi.Model.Parameters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asp.Net.Core.WebApi.CrudOperations.Controllers
{
    [Route("api/facets-categories")]
    [ApiController]
    public class FacetsController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<FacetsResponse> Get([FromQuery] FacetsQueryParameters parameters)
        {
            return Services.FacetsServiceProvider.GetAll(parameters);
        }
                

        [Route("{code}")]
        [HttpGet]
        public ActionResult<CategoriesResponse> Get(string code)
        {
            var response = Services.FacetsServiceProvider.GetByName(code);
            if (response != null)
            {
                return response;
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CategoriesItem accessCode)
        {
            var res = Services.FacetsServiceProvider.Create(accessCode);
            string? msg;
            if (res > 0)
            {
                msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Category has been successfully added!" });
            }
            else
            {
                msg = JsonConvert.SerializeObject(new { status = "FAIL", Message = "Something went wrong. Please Try later!" });
            }
            return Ok(msg);

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CategoriesItem accessCode)
        {
            if (id != accessCode.Id)
                return BadRequest();
            var objAccessCode = Services.FacetsServiceProvider.GetById(id);
            if (objAccessCode == null)
            {
                return NotFound();
            }
            var res = Services.FacetsServiceProvider.Update(id, accessCode);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Category has been updated successfully!" });
                return Ok(msg);
            }
            return NoContent();

        }

        //[Route("{category-name}/categories/{categoryId}/relation")]
        //[HttpGet]
        //public ActionResult<CategorieItem> GetAllFacetCategoryRelation(string categoryName,int categoryId)
        //{
        //    var accessCode = Services.FacetsServiceProvider.GetAllFacetCategoryRelation(id);
        //    if (accessCode != null)
        //    {
        //        return accessCode;
        //    }
        //    return NotFound();
        //}


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = Services.FacetsServiceProvider.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            var res = Services.FacetsServiceProvider.Remove(category);
            if (res)
            {
                var msg = JsonConvert.SerializeObject(new { status = "SUCCESS", Message = "Category has been deleted successfully!" });
                return Ok(msg);
            }
            return NoContent();
        }

        
        //[Route("{category-name}/categories/{categoryId}/mapping")]
        //[HttpGet]
        //public ActionResult<CategorieItem> GetFacetCategoryMapping(string categoryName,int categoryId)
        //{
        //    var accessCode = Services.FacetsServiceProvider.GetFacetCategoryMapping(id);
        //    if (accessCode != null)
        //    {
        //        return accessCode;
        //    }
        //    return NotFound();
        //}

        //[Route("{category-name}/categories/{categoryId}/publicationtitles")]
        //[HttpGet]
        //public ActionResult<CategorieItem> GetFacetCategoryPublicationTitles(string categoryName,int categoryId)
        //{
        //    var accessCode = Services.FacetsServiceProvider.GetFacetCategoryPublicationTitles(id);
        //    if (accessCode != null)
        //    {
        //        return accessCode;
        //    }
        //    return NotFound();
        //}

    }
}

using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.Drug;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtraDrug.Controllers;
[Route("api/drug-categories")]
[ApiController]
public class DrugCategoryController : ControllerBase
{
    private readonly IDrugCategoryRepo drugCategoryRepo;

    public DrugCategoryController(IDrugCategoryRepo _drugCategoryRepo)
    {
        drugCategoryRepo = _drugCategoryRepo;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugCategory(NameAndIdResource drugCategoryResource)
    {

        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> EditDrugCategory()
    {
        return Ok();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugCategory([FromRoute]int id)
    {
        var category = await drugCategoryRepo.DeleteDrugCategory(id);
        if (category is null) return BadRequest(new ErrorResponce()
        {
            Message="Category Id Is Invalid",
            Errors = new string[] { "Category Id Is Invalid" }
        });
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugCategories()
    {
        var categories = await drugCategoryRepo.GetAllDrugCategories();
        return Ok(new SuccessResponce<ICollection<NameAndIdResource>>()
        {
            Message = "All Drugs Categories",
            Data = categories.Select(c => NameAndIdResource.MapToResource(c)).ToList(),
            Meta = null
        });
    }
}

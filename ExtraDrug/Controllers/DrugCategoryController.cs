using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/drug-categories")]
[ApiController]
[ValidateModel]
[Authorize(Roles = "Admin")]
public class DrugCategoryController : ControllerBase
{
    private readonly IDrugCategoryRepo drugCategoryRepo;

    public DrugCategoryController(IDrugCategoryRepo _drugCategoryRepo)
    {
        drugCategoryRepo = _drugCategoryRepo;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugCategory([FromBody] NameAndIdResource drugCategoryResource)
    {
        var category = await drugCategoryRepo.AddDrugCategory(drugCategoryResource.MapToModel<DrugCategory>());
        return Created("",new SuccessResponce<NameAndIdResource>(){
            Message = "Created Successfuly",
            Data = NameAndIdResource.MapToResource(category),
            Meta = null
        });
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugCategory([FromRoute] int id ,[FromBody] NameAndIdResource drugCategoryResource)
    {
        var category = await drugCategoryRepo.UpdateDrugCategory(id,drugCategoryResource.MapToModel<DrugCategory>());
        if (category is null) return BadRequest(new ErrorResponce()
            {
                Message="Category Not Found",
                Errors = new string[] { "Category Id Is Invalid" }
            });
        return Ok(new SuccessResponce<NameAndIdResource>()
        {
            Message = "Updated Successfuly",
            Data = NameAndIdResource.MapToResource(category),
            Meta = null
        });
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
        return Ok(
                new SuccessResponce<NameAndIdResource>()
                {
                    Message= "Deleted Successfuly",
                    Data =  NameAndIdResource.MapToResource(category),
                    Meta = null
                }
            );
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

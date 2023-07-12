using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/drug-categories")]
[ApiController]
[ValidateModel]
[ExceptionHandler]

[Authorize(Roles = "Admin")]
public class DrugCategoryController : ControllerBase
{
    private readonly IDrugCategoryRepo _drugCategoryRepo;
    private readonly ResponceBuilder _responceBuilder;

    public DrugCategoryController(IDrugCategoryRepo drugCategoryRepo ,ResponceBuilder responceBuilder )
    {
        _drugCategoryRepo = drugCategoryRepo;
        _responceBuilder = responceBuilder;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugCategory([FromBody] NameAndIdResource drugCategoryResource)
    {
        var category = await _drugCategoryRepo.AddDrugCategory(drugCategoryResource.MapToModel<DrugCategory>());
        return Created("",_responceBuilder.CreateSuccess(message: "Created Successfuly", data: NameAndIdResource.MapToResource(category)));
        
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugCategory([FromRoute] int id ,[FromBody] NameAndIdResource drugCategoryResource)
    {
        var res  = await _drugCategoryRepo.UpdateDrugCategory(id,drugCategoryResource.MapToModel<DrugCategory>());
        if ( !res.IsSucceeded|| res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                message: "Category Not Found", errors: res.Errors 
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Updated Successfuly" ,
            data: NameAndIdResource.MapToResource(res.Data)
            ));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> getById([FromRoute] int id)
    {
        var res = await _drugCategoryRepo.GetCategoryById(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                message: "Category Not Found", errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Category Fetched",
            data: NameAndIdResource.MapToResource(res.Data)
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugCategory([FromRoute]int id)
    {
        var res = await _drugCategoryRepo.DeleteDrugCategory(id);
        if (!res.IsSucceeded || res.Data is null) 
            return NotFound(_responceBuilder.CreateFailure(
                message: "Category Not Found", errors:res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
         message: "Deleted Successfuly",
         data: NameAndIdResource.MapToResource(res.Data)
         ));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugCategories()
    {
        var categories = await _drugCategoryRepo.GetAllDrugCategories();
        return Ok(_responceBuilder.CreateSuccess(
            message: "All Drugs Categories",
            data: categories.Select(c => NameAndIdResource.MapToResource(c)).ToList()
            ));
    }
}

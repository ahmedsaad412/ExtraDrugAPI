using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExtraDrug.Controllers;
[Route("api/drug-types")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
[Authorize(Roles = "Admin")]
public class DrugTypesController : ControllerBase
{
    private readonly IDrugTypeRepo _drugTypeRepo;
    private readonly ResponceBuilder _responceBuilder;

    public DrugTypesController(IDrugTypeRepo drugTypeRepo , ResponceBuilder responceBuilder)
    {
        _drugTypeRepo = drugTypeRepo;
        _responceBuilder = responceBuilder;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugType([FromBody] NameAndIdResource drugTypeResource)
    {
        var type = await _drugTypeRepo.AddDrugType(drugTypeResource.MapToModel<DrugType>());
        return Created("", _responceBuilder.CreateSuccess(
             message : "Created Successfuly",
             data : NameAndIdResource.MapToResource(type)
            )); 
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugType([FromRoute] int id ,[FromBody] NameAndIdResource drugTypeResource)
    {
        var type = await _drugTypeRepo.UpdateDrugType(id, drugTypeResource.MapToModel<DrugType>());
        if (type is null) return NotFound(_responceBuilder.CreateFailure(
             message : "Type Not Found",
             errors : new string[] { "Type Id Is Invalid" }
            ));
        return Ok(_responceBuilder.CreateSuccess(
             message : "Updated Successfuly",
             data : NameAndIdResource.MapToResource(type)
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugType([FromRoute] int id)
    {
        var type = await _drugTypeRepo.DeleteDrugType(id);
        if (type is null) return NotFound(_responceBuilder.CreateFailure(
           message: "Type Not Found",
           errors: new string[] { "Type Id Is Invalid" }
          ));
        return Ok(_responceBuilder.CreateSuccess(
             message: "deleted Successfuly",
             data: NameAndIdResource.MapToResource(type)
           ));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugType()
    {
        var types = await _drugTypeRepo.GetAllDrugType();
        return Ok(_responceBuilder.CreateSuccess(
             message : "All Drugs types",
             data : types.Select(t => NameAndIdResource.MapToResource(t)).ToList()
            ));
    }
}

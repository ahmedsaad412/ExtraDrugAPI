using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        var res = await _drugTypeRepo.UpdateDrugType(id, drugTypeResource.MapToModel<DrugType>());
        if (!res.IsSucceeded || res.Data is null) return NotFound(_responceBuilder.CreateFailure(
             message : "Type Not Found",
             errors : res.Errors
            ));
        return Ok(_responceBuilder.CreateSuccess(
             message : "Updated Successfuly",
             data : NameAndIdResource.MapToResource(res.Data)
            ));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> getById([FromRoute] int id)
    {
        var res = await _drugTypeRepo.GetTypeById(id);
        if (!res.IsSucceeded || res.Data is null) return NotFound(_responceBuilder.CreateFailure(
             message: "Type Not Found",
             errors: res.Errors
            ));
        return Ok(_responceBuilder.CreateSuccess(
             message: "Type fetched",
             data: NameAndIdResource.MapToResource(res.Data)
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugType([FromRoute] int id)
    {
        var res = await _drugTypeRepo.DeleteDrugType(id);
        if (!res.IsSucceeded || res.Data is null) return NotFound(_responceBuilder.CreateFailure(
           message: "Type Not Found",
           errors: res.Errors
          ));
        return Ok(_responceBuilder.CreateSuccess(
             message: "deleted Successfuly",
             data: NameAndIdResource.MapToResource(res.Data)
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

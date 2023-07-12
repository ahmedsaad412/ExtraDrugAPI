using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/[controller]")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
[Authorize(Roles = "Admin")]
public class DrugController : ControllerBase
{
    private readonly IDrugRepo _drugRepo;
    private readonly ResponceBuilder _responceBuilder;

    public DrugController(IDrugRepo drugRepo , ResponceBuilder responceBuilder)
    {
        _drugRepo = drugRepo;
        _responceBuilder = responceBuilder;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDrug ([FromBody] SaveDrugResource sDrugR)
    {
        var drug = await _drugRepo.AddDrug(sDrugR.MapToModel());
        return Created("",_responceBuilder.CreateSuccess
        (
            message:"Created Successfully",
            data : DrugResource.MapToResource(drug)
        ));;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var res = await _drugRepo.GetDrugById(id, includeData:true);
        if(!res.IsSucceeded || res.Data is null)
        {
            return NotFound(_responceBuilder.CreateFailure
            (
                message : "Drug Not Found",
                errors : res.Errors
            ));
        }
        return Ok(_responceBuilder.CreateSuccess
        (
            message: "Drug Fetched",
            data: DrugResource.MapToResource(res.Data)
        ));
        

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrug([FromRoute] int id)
    {
        var res =  await _drugRepo.DeleteDrug(id);
        if (!res.IsSucceeded || res.Data is null)
        {
            return NotFound(_responceBuilder.CreateFailure
            (
                message: "Drug Not Found",
                errors: res.Errors
            ));
        }
        return Ok(_responceBuilder.CreateSuccess
        (
            message: "Drug Deleted",
            data: DrugResource.MapToResource(res.Data)
        ));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrgus()
    {
        var drugs = await _drugRepo.GetAllDrugs();

        return Ok(_responceBuilder.CreateSuccess
            (
                message : "All Drugs Fetched",
                data : drugs.Select(d=> DrugResource.MapToResource(d)).ToList()
            ));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDrug([FromRoute] int id , [FromBody] SaveDrugResource sDrugR )
    {
        var res = await _drugRepo.UpdateDrug(id, sDrugR.MapToModel());
        if (!res.IsSucceeded ||res.Data is null)
        {
            return NotFound(_responceBuilder.CreateFailure
            (
                message: "Drug Not Found",
                errors: res.Errors
            ));
        }
        return Ok(_responceBuilder.CreateSuccess
        (
            message: "Drug Updated",
            data: DrugResource.MapToResource(res.Data)
        ));
    }
}
 
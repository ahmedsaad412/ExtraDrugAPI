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
[Route("api/[controller]")]
[ApiController]
[ValidateModel]
[Authorize(Roles = "Admin")]
public class DrugController : ControllerBase
{
    private readonly IDrugRepo drugRepo;

    public DrugController(IDrugRepo _drugRepo)
    {
        drugRepo = _drugRepo;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDrug ([FromBody] SaveDrugResource sDrugR)
    {
        var drug = await drugRepo.AddDrug(sDrugR.MapToModel());
        return Created("",new SuccessResponce<DrugResource>()
        {
            Message = "Created Successfully",
            Data = DrugResource.MapToResource(drug),
            Meta = null
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var drug = await drugRepo.GetDrugById(id, includeData:true);
        if(drug is null)
        {
            return NotFound(new ErrorResponce()
            {
                Message = "Drug Not Found",
                Errors = new[] { "Drug Not Found" }
            });
        }
        else
        {
            return Ok(new SuccessResponce<DrugResource>()
            {
                Message = "Drug Fetched",
                Data = DrugResource.MapToResource(drug),
                Meta = null,
            });
        }

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrug([FromRoute] int id)
    {
        var drug =  await drugRepo.DeleteDrug(id);
        if (drug is null)
        {
            return NotFound(new ErrorResponce()
            {
                Message = "Drug Not Found",
                Errors = new[] { "Drug Not Found" }
            });
        }
        else
        {
            return Ok(new SuccessResponce<DrugResource>()
            {
                Message = "Drug Deleted",
                Data = DrugResource.MapToResource(drug),
                Meta = null,
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrgus()
    {
        var drugs = await drugRepo.GetAllDrugs();

        return Ok(new SuccessResponce<ICollection<DrugResource>>()
        {
            Message = "All Drugs Fetched",
            Data = drugs.Select(d=> DrugResource.MapToResource(d)).ToList(),
            Meta = null,
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDrug([FromRoute] int id , [FromBody] SaveDrugResource sDrugR )
    {
        var drug = await drugRepo.UpdateDrug(id, sDrugR.MapToModel());
        if (drug is null)
        {
            return NotFound(new ErrorResponce()
            {
                Message = "Drug Not Found",
                Errors = new[] { "Drug Not Found" }
            });
        }
        else
        {
            return Ok(new SuccessResponce<DrugResource>()
            {
                Message = "Drug Updated",
                Data = DrugResource.MapToResource(drug),
                Meta = null,
            });
        }
    }
}
 
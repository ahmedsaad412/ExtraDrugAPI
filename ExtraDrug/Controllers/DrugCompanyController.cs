using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/drug-companies")]
[ApiController]
[ValidateModel]
[ExceptionHandler]

[Authorize(Roles = "Admin")]
public class DrugCompanyController : ControllerBase
{
    private readonly IDrugCompanyRepo _drugCompanyRepo;
    private readonly ResponceBuilder _responceBuilder;

    public DrugCompanyController(IDrugCompanyRepo drugCompanyRepo , ResponceBuilder responceBuilder)
    {
        _drugCompanyRepo = drugCompanyRepo;
        _responceBuilder = responceBuilder;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugCompany([FromBody] NameAndIdResource drugTypeResource)
    {
        var company = await _drugCompanyRepo.AddDrugCompany(drugTypeResource.MapToModel<DrugCompany>());
        return Created("", _responceBuilder.CreateSuccess(
            message:"Created Successfuly" ,
            data:NameAndIdResource.MapToResource(company)));
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugCompany([FromRoute] int id ,[FromBody] NameAndIdResource drugTypeResource)
    {
        var res = await _drugCompanyRepo.UpdateDrugCompany(id, drugTypeResource.MapToModel<DrugCompany>());
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                 message: "company Not Found",
                 errors:res.Errors
                )) ;
        return Ok(_responceBuilder.CreateSuccess(
                message : "Updated Successfuly",
                data : NameAndIdResource.MapToResource(res.Data)
            )); 
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> getById([FromRoute] int id)
    {
        var res = await _drugCompanyRepo.GetCompanyById(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                 message: "company Not Found",
                 errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
                message: "company fetced",
                data: NameAndIdResource.MapToResource(res.Data)
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugCompany([FromRoute] int id)
    {
        var res = await _drugCompanyRepo.DeleteDrugCompany(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                 message: "company Not Found",
                 errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
                message: "Deleted Successfuly",
                data: NameAndIdResource.MapToResource(res.Data)
            ));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugCompany()
    {
        var companies = await _drugCompanyRepo.GetAllDrugCompany();
        return Ok(_responceBuilder.CreateSuccess(
            message : "All Drugs companies",
            data : companies.Select(t => NameAndIdResource.MapToResource(t)).ToList()
            )); 
    }
}

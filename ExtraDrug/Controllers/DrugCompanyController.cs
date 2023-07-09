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
[Route("api/drug-companies")]
[ApiController]
[ValidateModel]
[Authorize(Roles = "Admin")]
public class DrugCompanyController : ControllerBase
{
    private readonly IDrugCompanyRepo _drugCompanyRepo;

    public DrugCompanyController(IDrugCompanyRepo drugCompanyRepo)
    {
        _drugCompanyRepo = drugCompanyRepo;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugCompany([FromBody] NameAndIdResource drugTypeResource)
    {
        var company = await _drugCompanyRepo.AddDrugCompany(drugTypeResource.MapToModel<DrugCompany>());
        return Created("",new SuccessResponce<NameAndIdResource>(){
            Message = "Created Successfuly",
            Data = NameAndIdResource.MapToResource(company),
            Meta = null
        });
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugCompany([FromRoute] int id ,[FromBody] NameAndIdResource drugTypeResource)
    {
        var company = await _drugCompanyRepo.UpdateDrugCompany(id, drugTypeResource.MapToModel<DrugCompany>());
        if (company is null) return BadRequest(new ErrorResponce()
            {
                Message= "company Not Found",
                Errors = new string[] { "company Id Is Invalid" }
            });
        return Ok(new SuccessResponce<NameAndIdResource>()
        {
            Message = "Updated Successfuly",
            Data = NameAndIdResource.MapToResource(company),
            Meta = null
        });
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugCompany([FromRoute] int id)
    {
        var company = await _drugCompanyRepo.DeleteDrugCompany(id);
        if (company is null) return BadRequest(new ErrorResponce()
        {
            Message= "company Id Is Invalid",
            Errors = new string[] { "company Id Is Invalid" }
        });
        return Ok(
                new SuccessResponce<NameAndIdResource>()
                {
                    Message= "Deleted Successfuly",
                    Data =  NameAndIdResource.MapToResource(company),
                    Meta = null
                }
            );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugCompany()
    {
        var companies = await _drugCompanyRepo.GetAllDrugCompany();
        return Ok(new SuccessResponce<ICollection<NameAndIdResource>>()
        {
            Message = "All Drugs companies",
            Data = companies.Select(t => NameAndIdResource.MapToResource(t)).ToList(),
            Meta = null
        });
    }
}

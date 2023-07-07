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
[Route("api/drug-types")]
[ApiController]
[ValidateModel]
[Authorize(Roles = "Admin")]
public class DrugTypesController : ControllerBase
{
    private readonly IDrugTypeRepo drugTypeRepo;

    public DrugTypesController(IDrugTypeRepo _drugTypeRepo)
    {
        drugTypeRepo = _drugTypeRepo;
    }
    [HttpPost]
    public async Task<IActionResult> AddDrugType([FromBody] NameAndIdResource drugTypeResource)
    {
        var type = await drugTypeRepo.AddDrugType(drugTypeResource.MapToModel<DrugType>());
        return Created("",new SuccessResponce<NameAndIdResource>(){
            Message = "Created Successfuly",
            Data = NameAndIdResource.MapToResource(type),
            Meta = null
        });
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditDrugType([FromRoute] int id ,[FromBody] NameAndIdResource drugTypeResource)
    {
        var type = await drugTypeRepo.UpdateDrugType(id, drugTypeResource.MapToModel<DrugType>());
        if (type is null) return BadRequest(new ErrorResponce()
            {
                Message= "Type Not Found",
                Errors = new string[] { "Type Id Is Invalid" }
            });
        return Ok(new SuccessResponce<NameAndIdResource>()
        {
            Message = "Updated Successfuly",
            Data = NameAndIdResource.MapToResource(type),
            Meta = null
        });
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDrugType([FromRoute] int id)
    {
        var type = await drugTypeRepo.DeleteDrugType(id);
        if (type is null) return BadRequest(new ErrorResponce()
        {
            Message= "type Id Is Invalid",
            Errors = new string[] { "type Id Is Invalid" }
        });
        return Ok(
                new SuccessResponce<NameAndIdResource>()
                {
                    Message= "Deleted Successfuly",
                    Data =  NameAndIdResource.MapToResource(type),
                    Meta = null
                }
            );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrugType()
    {
        var types = await drugTypeRepo.GetAllDrugType();
        return Ok(new SuccessResponce<ICollection<NameAndIdResource>>()
        {
            Message = "All Drugs types",
            Data = types.Select(t => NameAndIdResource.MapToResource(t)).ToList(),
            Meta = null
        });
    }
}

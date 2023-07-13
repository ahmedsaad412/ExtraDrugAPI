using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/effective-matrials")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
[Authorize(Roles = "Admin")]
public class EffectiveMatrialsController : ControllerBase
{
    private readonly IEffectiveMatrialRepo _effectiveMatrialRepo;
    private readonly ResponceBuilder _responceBuilder;

    public EffectiveMatrialsController(IEffectiveMatrialRepo effectiveMatrialRepo , ResponceBuilder responceBuilder)
    {
        _effectiveMatrialRepo = effectiveMatrialRepo;
        _responceBuilder = responceBuilder;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var martrials= await _effectiveMatrialRepo.GetAll();
        return Ok(_responceBuilder.CreateSuccess(
                message: "All Effective Matrials Fetched",
                data:  martrials.Select(m => NameAndIdResource.MapToResource(m)).ToList()
            )); 
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id )
    {
        var res = await _effectiveMatrialRepo.GetById(id);
        if ( !res.IsSucceeded || res.Data == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors:res.Errors
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial fetched",
            data: NameAndIdResource.MapToResource(res.Data)
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> delete([FromRoute] int id)
    {
        var res = await _effectiveMatrialRepo.Delete(id);
        if (!res.IsSucceeded || res.Data == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors:res.Errors
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial deleted",
            data: NameAndIdResource.MapToResource(res.Data)
            ));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> update([FromRoute] int id , [FromBody] NameAndIdResource efNameRes)
    {
        var res = await _effectiveMatrialRepo.Update(id, efNameRes.MapToModel<EffectiveMatrial>());
        if (!res.IsSucceeded ||res.Data == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors: res.Errors
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial updated",
            data: NameAndIdResource.MapToResource(res.Data)
            ));
    }

    [HttpPost]
    public async Task<IActionResult> add([FromBody] NameAndIdResource efNameRes)
    {
        var matrial = await _effectiveMatrialRepo.Add( efNameRes.MapToModel<EffectiveMatrial>());
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial Created",
            data: NameAndIdResource.MapToResource(matrial)
        ));
    }
}

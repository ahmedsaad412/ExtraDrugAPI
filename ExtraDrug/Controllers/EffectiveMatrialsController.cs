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
        var matrial = await _effectiveMatrialRepo.GetById(id);
        if (matrial == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors: new[] { "provided id invalid"}
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial fetched",
            data:matrial
            ));
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> delete([FromRoute] int id)
    {
        var matrial = await _effectiveMatrialRepo.Delete(id);
        if (matrial == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors: new[] { "provided id invalid" }
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial deleted",
            data: matrial
            ));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> update([FromRoute] int id , [FromBody] NameAndIdResource efNameRes)
    {
        var matrial = await _effectiveMatrialRepo.Update(id, efNameRes.MapToModel<EffectiveMatrial>());
        if (matrial == null) return NotFound(_responceBuilder.CreateFailure(
                message: "Effective Matrial Not Found",
                errors: new[] { "provided id invalid" }
            ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial updated",
            data: matrial
            ));
    }

    [HttpPost]
    public async Task<IActionResult> add([FromBody] NameAndIdResource efNameRes)
    {
        var matrial = await _effectiveMatrialRepo.Add( efNameRes.MapToModel<EffectiveMatrial>());
        return Ok(_responceBuilder.CreateSuccess(
            message: "Effective Matrial Created",
            data: matrial
        ));
    }
}

using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
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
}

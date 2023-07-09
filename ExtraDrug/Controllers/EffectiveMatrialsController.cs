using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtraDrug.Controllers;
[Route("api/effective-matrials")]
[ApiController]
[ValidateModel]
[Authorize(Roles = "Admin")]
public class EffectiveMatrialsController : ControllerBase
{
    private readonly IEffectiveMatrialRepo _effectiveMatrialRepo;

    public EffectiveMatrialsController(IEffectiveMatrialRepo effectiveMatrialRepo)
    {
        _effectiveMatrialRepo = effectiveMatrialRepo;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var martrials= await _effectiveMatrialRepo.GetAll();
        return Ok(new SuccessResponce<ICollection<NameAndIdResource>>()
        {
            Message = "All Effective Matrials Fetched",
            Data = martrials.Select(m => NameAndIdResource.MapToResource(m)).ToList(),
            Meta = null
        });
    }
}

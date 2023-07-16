using ExtraDrug.Controllers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtraDrug.Controllers;
[Route("api/user/requests")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
public class DrugRequestController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> AddDrugRequest()
    {
        return Ok();
    }
}

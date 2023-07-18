using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.DrugRequestResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtraDrug.Controllers;
[Route("api/user/requests")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
[Authorize]
public class DrugRequestController : ControllerBase
{
    private readonly IDrugRequestRepo _drugRequestRepo;
    private readonly ResponceBuilder _responceBuilder;

    public DrugRequestController(IDrugRequestRepo drugRequestRepo, ResponceBuilder responceBuilder)
    {
        _drugRequestRepo = drugRequestRepo;
        _responceBuilder = responceBuilder;
    }
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> AddDrugRequest(AddDrugRequestResource drr)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var dr = drr.MapToModel();
        var res = await _drugRequestRepo.AddDrugRequest(userIdFromToken, dr);

        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Request Can't be Added.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "drug request added",
            data: DrugRequestResource.MapToResource(res.Data)
            ));

    }

    [HttpGet("as_donor")]
    public async Task<IActionResult> GetAllUsersRequestsAsDonor()
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _drugRequestRepo.GetAllUsersRequests(userIdFromToken, IsDonor: true);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message:  "Can't Fetch Requests.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "drug requests Fetched",
            data: res.Data.Select(DrugRequestResource.MapToResource)
            ));
    }
    [HttpGet("as_reciever")]
    public async Task<IActionResult> GetAllUsersRequestsAsReciever()
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _drugRequestRepo.GetAllUsersRequests(userIdFromToken, IsDonor: false);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't Fetch Requests.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "drug requests Fetched",
            data: res.Data.Select(DrugRequestResource.MapToResource)
            ));
    }



    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRequestById([FromRoute]int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();
        var res =  await _drugRequestRepo.GetDrugRequestById(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Request NotFound",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Request Featched",
            data: DrugRequestResource.MapToResource(res.Data)
        ));
    }

    [HttpPatch("{id:int}/accept")]
    public async Task<IActionResult> AcceptRequest([FromRoute] int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");
        if (userIdFromToken is null)
            return Forbid();
        var res = await _drugRequestRepo.UpdateDrugRequestState(userIdFromToken, id, RequestState.Accepted);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't Accept request.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Request Accepted",
            data: DrugRequestResource.MapToResource(res.Data)
        ));
    }

    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> CancelRequest([FromRoute] int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");
        if (userIdFromToken is null)
            return Forbid();
        var res = await _drugRequestRepo.UpdateDrugRequestState(userIdFromToken, id, RequestState.Canceled);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't cancel the request.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Request canceled",
            data: DrugRequestResource.MapToResource(res.Data)
        ));
    }

    [HttpPatch("{id:int}/recieve")]
    public async Task<IActionResult> RecieveRequest([FromRoute] int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");
        if (userIdFromToken is null)
            return Forbid();
        var res = await _drugRequestRepo.UpdateDrugRequestState(userIdFromToken, id, RequestState.Recieved);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't recive the request.",
                    errors: res.Errors
                ));

        return Ok(_responceBuilder.CreateSuccess(
            message: "Request recived",
            data: DrugRequestResource.MapToResource(res.Data)
        ));
    }
}

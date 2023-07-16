using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRequestRepo : IDrugRequestRepo
{
    private readonly IUserRepo _userRepo;
    private readonly RepoResultBuilder<DrugRequest> _repoResultBuilder;
    private readonly AppDbContext _ctx;

    public DrugRequestRepo(IUserRepo userRepo, RepoResultBuilder<DrugRequest> repoResultBuilder , AppDbContext ctx)
    {
        _userRepo = userRepo;
        _repoResultBuilder = repoResultBuilder;
        _ctx = ctx;
    }
    public async Task<RepoResult<DrugRequest>> AddDrugRequest(string userId,  DrugRequest dr)
    {

        var res = await _userRepo.GetById(userId);
        if (!res.IsSucceeded || res.Data is null) return _repoResultBuilder.Failuer(new[] { "Applicant User NotFound" });
        dr.ReceiverId = res.Data.Id;
        dr.State = RequestState.Pending;
        dr.CreatedAt = DateTime.UtcNow;
        dr.LastUpdatedAt = DateTime.UtcNow;
        _ctx.DrugRequests.Add(dr);
        await _ctx.SaveChangesAsync();
        return _repoResultBuilder.Success(dr);
    }

    public Task<RepoResult<DrugRequest>> GetDrugRequestById(string userId, int drugRequestId)
    {
        throw new NotImplementedException();
    }

    public Task<RepoResult<DrugRequest>> UpdateDrugRequestItems(string userId, int drugRequestId, ICollection<RequestItem> items)
    {
        throw new NotImplementedException();
    }

    public Task<RepoResult<DrugRequest>> UpdateDrugRequestState(string userId, int drugRequestId, RequestState newState)
    {
        throw new NotImplementedException();
    }
}

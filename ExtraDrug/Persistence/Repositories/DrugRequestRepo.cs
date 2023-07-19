using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Helpers;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRequestRepo : IDrugRequestRepo
{
    private readonly IUserRepo _userRepo;
    private readonly RepoResultBuilder<DrugRequest> _repoResultBuilder;
    private readonly StateManager _stateManager;
    private readonly AppDbContext _ctx;

    public DrugRequestRepo(IUserRepo userRepo, RepoResultBuilder<DrugRequest> repoResultBuilder ,StateManager stateManager, AppDbContext ctx)
    {
        _userRepo = userRepo;
        _repoResultBuilder = repoResultBuilder;
        _stateManager = stateManager;
        _ctx = ctx;
    }
    public async Task<RepoResult<DrugRequest>> AddDrugRequest(string userId,  DrugRequest dr)
    {
        var recieverRes = await _userRepo.GetByIdWithoutDate(userId);
        if (!recieverRes.IsSucceeded || recieverRes.Data is null) return _repoResultBuilder.Failuer(new[] { "Applicant User NotFound" });
        dr.Receiver = recieverRes.Data;

        var donorRes = await _userRepo.GetById(dr.DonorId);
        if (!donorRes.IsSucceeded || donorRes.Data is null) return _repoResultBuilder.Failuer(new[] { "Donor User NotFound" });
        dr.Donor = donorRes.Data;

        foreach (var ri in dr.RequestItems)
        {
            var userDrug = dr.Donor.UserDrugs.SingleOrDefault(ud => ud.Id == ri.UserDrugId);
            if(userDrug is null ) return _repoResultBuilder.Failuer(new[] { "Donor Didn't have this Drug." });
            if(userDrug.Quantity < ri.Quantity) return _repoResultBuilder.Failuer(new[] { "Donor Didn't have this Quantity of this Drug." });
        }

        dr.State = RequestState.Pending;
        dr.CreatedAt = DateTime.UtcNow;
        dr.LastUpdatedAt = DateTime.UtcNow;
        _ctx.DrugRequests.Add(dr);
        await _ctx.SaveChangesAsync();
        return await GetDrugRequestById(dr.Id);
    }

    public async Task<RepoResult<DrugRequest>> GetDrugRequestById(int drugRequestId)
    {
        var dr = await _ctx.DrugRequests
            .Include(dr => dr.Receiver)
            .Include(dr => dr.RequestItems).ThenInclude(ri=> ri.UserDrug).ThenInclude(ud=> ud.Drug)
            .Include(dr => dr.RequestItems).ThenInclude(ri=> ri.UserDrug).ThenInclude(ud=> ud.Photos)
            .AsNoTracking()
            .SingleOrDefaultAsync(dr => dr.Id == drugRequestId);
        if (dr is null) 
            return _repoResultBuilder.Failuer(new[] {"Drug Request Not Found. Invalid Id"});

        var res = await _userRepo.GetById(dr.DonorId);
        if(!res.IsSucceeded || res.Data is  null ) return _repoResultBuilder.Failuer(new[] { "Drug Request Donor Not Found" });
        dr.Donor = res.Data;
        return _repoResultBuilder.Success(dr);
    }

    public async Task<RepoResult<DrugRequest>> UpdateDrugRequestState(string userId, int drugRequestId, RequestState newState)
    {
        var dr = await _ctx.DrugRequests.SingleOrDefaultAsync(dr => dr.Id == drugRequestId);

        if (dr is null) return _repoResultBuilder.Failuer(new[] { "Drug Request Donor Not Found" });
        if (!dr.ReceiverId.Equals(userId) && !dr.DonorId.Equals(userId)) return _repoResultBuilder.Failuer(new[] {"User havn't permission to change state of the request."});

        if(newState == RequestState.Accepted && !dr.DonorId.Equals(userId))
            return _repoResultBuilder.Failuer(new[] { "Donor only can accept the request." });

        if (newState == RequestState.Recieved && !dr.ReceiverId.Equals(userId))
            return _repoResultBuilder.Failuer(new[] { "Reciever only can recieve the request." });

        if (!_stateManager.validStateChange(dr.State, newState))
            return _repoResultBuilder.Failuer(new[] { $"Invaid state change. can't change state from {dr.State} to {newState}"});
        dr.State = newState;
        dr.LastUpdatedAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();
      

        return await GetDrugRequestById(drugRequestId);
    }

    public async Task<RepoResult<ICollection<DrugRequest>>> GetAllUsersRequests(string userId, bool IsDonor)
    {
        var query  =  _ctx.DrugRequests.AsNoTracking().AsQueryable();
        if (IsDonor) query = query.Where(dr => dr.DonorId == userId);
        else query = query.Where(dr => dr.ReceiverId == userId);
        query = query
            .Include(dr => dr.Donor)
            .Include(dr => dr.Receiver)
            .Include(dr => dr.RequestItems).ThenInclude(ri => ri.UserDrug).ThenInclude(ud => ud.Drug)
            .Include(dr => dr.RequestItems).ThenInclude(ri => ri.UserDrug).ThenInclude(ud => ud.Photos)
            .OrderByDescending(dr => dr.LastUpdatedAt);
        return new RepoResult<ICollection<DrugRequest>>() {Data = await query.ToListAsync() , Errors=null , IsSucceeded=true};
    }

}

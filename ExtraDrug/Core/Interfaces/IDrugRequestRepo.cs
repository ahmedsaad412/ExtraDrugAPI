using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugRequestRepo
{
    Task<RepoResult<DrugRequest>> AddDrugRequest(string userId, DrugRequest dr);
    Task<RepoResult<DrugRequest>> UpdateDrugRequestState(string userId,int drugRequestId, RequestState newState);
    Task<RepoResult<DrugRequest>> UpdateDrugRequestItems(string userId, int drugRequestId, ICollection<RequestItem> items);
    Task<RepoResult<DrugRequest>> GetDrugRequestById(int drugRequestId);
    Task<RepoResult<ICollection<DrugRequest>>> GetAllUsersRequests(string userId, bool IsDonor);

}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRequestRepo : IDrugRequestRepo
{
    public Task<RepoResult<DrugRequest>> AddDrugRequest(DrugRequest dr)
    {
        throw new NotImplementedException();
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

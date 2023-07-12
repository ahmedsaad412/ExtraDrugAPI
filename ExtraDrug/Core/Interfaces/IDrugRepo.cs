using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugRepo
{
    Task<Drug> AddDrug(Drug d);
    Task<RepoResult<Drug>> GetDrugById(int id ,bool includeData);
    Task<ICollection<Drug>> GetAllDrugs();
    Task<RepoResult<Drug>> UpdateDrug(int id , Drug d);
    Task<RepoResult<Drug>> DeleteDrug(int id);
}

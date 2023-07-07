using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugRepo
{
    Task<Drug> AddDrug(Drug d);
    Task<Drug?> GetDrugById(int id ,bool includeData);
    Task<ICollection<Drug>> GetAllDrugs();
    Task<Drug?> UpdateDrug(int id , Drug d);
    Task<Drug?> DeleteDrug(int id);


}

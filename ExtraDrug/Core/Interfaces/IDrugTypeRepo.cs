using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugTypeRepo
{
    Task<DrugType> AddDrugType(DrugType drugType);
    Task<RepoResult<DrugType>> DeleteDrugType(int Id);
    Task<RepoResult<DrugType>> UpdateDrugType(int Id , DrugType drugType);
    Task<ICollection<DrugType>> GetAllDrugType();
    Task<RepoResult<DrugType>> GetTypeById(int id);
}

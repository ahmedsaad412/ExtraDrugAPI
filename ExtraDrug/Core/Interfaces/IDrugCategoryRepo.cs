using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugCategoryRepo
{
    Task<DrugCategory> AddDrugCategory(DrugCategory drugCategory);
    Task<RepoResult<DrugCategory>> DeleteDrugCategory(int Id);
    Task<RepoResult<DrugCategory>> UpdateDrugCategory(int Id , DrugCategory drugCategory);
    Task<ICollection<DrugCategory>> GetAllDrugCategories();
    Task<RepoResult<DrugCategory>> GetCategoryById(int id);
}

using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugCategoryRepo
{
    Task<DrugCategory> AddDrugCategory(DrugCategory drugCategory);
    Task<DrugCategory?> DeleteDrugCategory(int Id);
    Task<DrugCategory?> UpdateDrugCategory(int Id , DrugCategory drugCategory);
    Task<ICollection<DrugCategory>> GetAllDrugCategories();
    Task<DrugCategory?> GetCategoryById(int id);
}

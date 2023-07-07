using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugTypeRepo
{
    Task<DrugType> AddDrugType(DrugType drugType);
    Task<DrugType?> DeleteDrugType(int Id);
    Task<DrugType?> UpdateDrugType(int Id , DrugType drugType);
    Task<ICollection<DrugType>> GetAllDrugType();

}

using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugCompanyRepo
{
    Task<DrugCompany> AddDrugCompany(DrugCompany drugCompany);
    Task<DrugCompany?> DeleteDrugCompany(int Id);
    Task<DrugCompany?> UpdateDrugCompany(int Id , DrugCompany drugCompany);
    Task<ICollection<DrugCompany>> GetAllDrugCompany();
    Task<DrugCompany?> GetCompanyById(int id);


}

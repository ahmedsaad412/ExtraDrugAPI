using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IDrugCompanyRepo
{
    Task<DrugCompany> AddDrugCompany(DrugCompany drugCompany);
    Task<RepoResult<DrugCompany>> DeleteDrugCompany(int Id);
    Task<RepoResult<DrugCompany>> UpdateDrugCompany(int Id , DrugCompany drugCompany);
    Task<ICollection<DrugCompany>> GetAllDrugCompany();
    Task<RepoResult<DrugCompany>> GetCompanyById(int id);

}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCompanyRepo : IDrugCompanyRepo
{
    private readonly AppDbContext _ctx;
    private readonly RepoResultBuilder<DrugCompany> _repoResultBuilder;

    public DrugCompanyRepo(AppDbContext ctx, RepoResultBuilder<DrugCompany> repoResultBuilder)
    {
        _ctx = ctx;
        _repoResultBuilder = repoResultBuilder;
    }
    public async Task<DrugCompany> AddDrugCompany(DrugCompany drugCompany )
    {
        _ctx.DrugCompanies.Add(drugCompany);
        await _ctx.SaveChangesAsync();
        return drugCompany;
    }

    public async Task<RepoResult<DrugCompany>> DeleteDrugCompany(int Id)
    {

        var dc = await _ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return _repoResultBuilder.Failuer(new[] { "Company Id invalid. Company Not Fount " });
        _ctx.DrugCompanies.Remove(dc);
        await _ctx.SaveChangesAsync(); 
        return _repoResultBuilder.Success(dc);
    }

    public async Task<ICollection<DrugCompany>> GetAllDrugCompany()
    {
        return await _ctx.DrugCompanies.ToListAsync();
    }

    public async Task<RepoResult<DrugCompany>> GetCompanyById(int id)
    {
        var company =  await _ctx.DrugCompanies.FindAsync(id);
        if (company is null) return _repoResultBuilder.Failuer(new[] { "Company Id invalid. Company Not Fount " });
        return _repoResultBuilder.Success(company);
    }

    public async Task<RepoResult<DrugCompany>> UpdateDrugCompany(int Id, DrugCompany drugCompany)
    {
        var dc = await _ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return _repoResultBuilder.Failuer(new[] { "Company Id invalid. Company Not Fount " });
        dc.Name = drugCompany.Name;
        await _ctx.SaveChangesAsync();
        return _repoResultBuilder.Success(dc);
    }
}

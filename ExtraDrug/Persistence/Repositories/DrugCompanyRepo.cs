using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCompanyRepo : IDrugCompanyRepo
{
    private readonly AppDbContext _ctx;
    public DrugCompanyRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<DrugCompany> AddDrugCompany(DrugCompany drugCompany)
    {
        _ctx.DrugCompanies.Add(drugCompany);
        await _ctx.SaveChangesAsync();
        return drugCompany;
    }

    public async Task<DrugCompany?> DeleteDrugCompany(int Id)
    {

        var dc = await _ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        _ctx.DrugCompanies.Remove(dc);
        await _ctx.SaveChangesAsync(); ;
        return dc;
    }

    public async Task<ICollection<DrugCompany>> GetAllDrugCompany()
    {
        return await _ctx.DrugCompanies.ToListAsync();
    }

    public async Task<DrugCompany?> GetCompanyById(int id)
    {
        return await _ctx.DrugCompanies.FindAsync(id);
    }

    public async Task<DrugCompany?> UpdateDrugCompany(int Id, DrugCompany drugCompany)
    {
        var dc = await _ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        dc.Name = drugCompany.Name;
        await _ctx.SaveChangesAsync(); 
        return dc;
    }
}

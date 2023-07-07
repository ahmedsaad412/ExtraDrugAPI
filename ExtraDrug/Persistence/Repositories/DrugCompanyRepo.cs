using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCompanyRepo : IDrugCompanyRepo
{
    private readonly AppDbContext ctx;
    public DrugCompanyRepo(AppDbContext _ctx)
    {
        ctx = _ctx;
    }
    public async Task<DrugCompany> AddDrugCompany(DrugCompany drugCompany)
    {
        ctx.DrugCompanies.Add(drugCompany);
        await ctx.SaveChangesAsync();
        return drugCompany;
    }

    public async Task<DrugCompany?> DeleteDrugCompany(int Id)
    {

        var dc = await ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        ctx.DrugCompanies.Remove(dc);
        await ctx.SaveChangesAsync(); ;
        return dc;
    }

    public async Task<ICollection<DrugCompany>> GetAllDrugCompany()
    {
        return await ctx.DrugCompanies.ToListAsync();
    }

    public async Task<DrugCompany?> UpdateDrugCompany(int Id, DrugCompany drugCompany)
    {
        var dc = await ctx.DrugCompanies.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        dc.Name = drugCompany.Name;
        await ctx.SaveChangesAsync(); 
        return dc;
    }
}

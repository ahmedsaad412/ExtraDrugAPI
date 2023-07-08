using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRepo : IDrugRepo
{
    private readonly AppDbContext ctx;

    public DrugRepo(AppDbContext _ctx)
    {
        ctx = _ctx;
    }
    public async Task<Drug> AddDrug(Drug d)
    {
        var efMats = d.EffectiveMatrials;
        d.EffectiveMatrials = new List<EffectiveMatrial>(); 
        ctx.Drugs.Add(d);
        foreach (var ef in efMats)
        {
            if (ef is null) continue;

            if (ef.Id == 0)
            {
                ctx.EffectiveMatrials.Add(ef);
                ef.InDrugs.Add(d);
            }
            else
            {
                var ef_from_db = await ctx.EffectiveMatrials.FindAsync(ef.Id);
                ef_from_db?.InDrugs.Add(d);
            }

        }
        await ctx.SaveChangesAsync();
        await ctx.Entry(d).Reference(d =>d.Company).LoadAsync();
        await ctx.Entry(d).Reference(d => d.DrugCategory).LoadAsync();
        await ctx.Entry(d).Reference(d => d.DrugType).LoadAsync();
        await ctx.Entry(d).Collection(d => d.EffectiveMatrials).LoadAsync();
        return d; 
    }

    public async Task<Drug?> DeleteDrug(int id)
    {
        var drug = await GetDrugById(id, includeData:true);
        if (drug == null) return null;
        ctx.Remove(drug);
        await ctx.SaveChangesAsync();
        return drug;
    }

    public async Task<ICollection<Drug>> GetAllDrugs()
    {
        return await ctx.Drugs
                .Include(d => d.Company)
                .Include(d => d.DrugType)
                .Include(d => d.DrugCategory)
                .Include(d => d.EffectiveMatrials).ToListAsync();
    }

    public async Task<Drug?> GetDrugById(int id , bool includeData)
    {
        if(!includeData)
        {
            return await ctx.Drugs.SingleOrDefaultAsync(d => d.Id == id);
        }
        else
        {
            return await ctx.Drugs
                .Include(d=>d.Company)
                .Include(d=>d.DrugType)
                .Include(d=>d.DrugCategory)
                .Include(d=>d.EffectiveMatrials)
                .SingleOrDefaultAsync(d => d.Id == id);
        }
    }

    public async  Task<Drug?> UpdateDrug(int id, Drug d)
    {
        var drug = await GetDrugById(id, includeData: true);
        if (drug == null) return null;
       
        return drug;
    }
}

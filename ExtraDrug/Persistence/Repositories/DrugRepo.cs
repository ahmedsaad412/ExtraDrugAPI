using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRepo : IDrugRepo
{
    private readonly AppDbContext _ctx;

    public DrugRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Drug> AddDrug(Drug d)
    {
        var efMats = d.EffectiveMatrials;
        d.EffectiveMatrials = new List<EffectiveMatrial>(); 
        _ctx.Drugs.Add(d);
        foreach (var ef in efMats)
        {
            if (ef is null) continue;

            if (ef.Id == 0)
            {
                _ctx.EffectiveMatrials.Add(ef);
                ef.InDrugs.Add(d);
            }
            else
            {
                var ef_from_db = await _ctx.EffectiveMatrials.FindAsync(ef.Id);
                ef_from_db?.InDrugs.Add(d);
            }

        }
        await _ctx.SaveChangesAsync();
        await _ctx.Entry(d).Reference(d =>d.Company).LoadAsync();
        await _ctx.Entry(d).Reference(d => d.DrugCategory).LoadAsync();
        await _ctx.Entry(d).Reference(d => d.DrugType).LoadAsync();
        await _ctx.Entry(d).Collection(d => d.EffectiveMatrials).LoadAsync();
        return d; 
    }

    public async Task<Drug?> DeleteDrug(int id)
    {
        var drug = await GetDrugById(id, includeData:true);
        if (drug == null) return null;
        _ctx.Remove(drug);
        await _ctx.SaveChangesAsync();
        return drug;
    }

    public async Task<ICollection<Drug>> GetAllDrugs()
    {
        return await _ctx.Drugs
                .Include(d => d.Company)
                .Include(d => d.DrugType)
                .Include(d => d.DrugCategory)
                .Include(d => d.EffectiveMatrials).ToListAsync();
    }

    public async Task<Drug?> GetDrugById(int id , bool includeData)
    {
        if(!includeData)
        {
            return await _ctx.Drugs.SingleOrDefaultAsync(d => d.Id == id);
        }
        else
        {
            return await _ctx.Drugs
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


        drug.Ar_Name = d.Ar_Name;
        drug.En_Name = d.En_Name;
        drug.Parcode= d.Parcode;
        drug.Purpose = d.Purpose;
        drug.IsTradingPermitted = d.IsTradingPermitted;
        drug.CategoryId = d.CategoryId;
        drug.CompanyId = d.CompanyId;
        drug.TypeId = d.TypeId;
        drug.EffectiveMatrials.Clear();
        
        foreach (var ef in d.EffectiveMatrials)
        {
            if (ef is null) continue;

            if (ef.Id == 0)
            {
                _ctx.EffectiveMatrials.Add(ef);
                ef.InDrugs.Add(drug);
            }
            else
            {
                var ef_from_db = await _ctx.EffectiveMatrials.FindAsync(ef.Id);
                ef_from_db?.InDrugs.Add(drug);
            }

        }

        await _ctx.SaveChangesAsync();
        await _ctx.Entry(drug).Reference(d => d.Company).LoadAsync();
        await _ctx.Entry(drug).Reference(d => d.DrugCategory).LoadAsync();
        await _ctx.Entry(drug).Reference(d => d.DrugType).LoadAsync();
        await _ctx.Entry(drug).Collection(d => d.EffectiveMatrials).LoadAsync();
        return drug;
    }
}

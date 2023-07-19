using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Migrations;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugRepo : IDrugRepo
{
    private readonly AppDbContext _ctx;
    private readonly RepoResultBuilder<Drug> _repoResultBuilder;

    public DrugRepo(AppDbContext ctx, RepoResultBuilder<Drug> repoResultBuilder)
    {
        _ctx = ctx;
        _repoResultBuilder = repoResultBuilder;
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

    public async Task<RepoResult<Drug>> DeleteDrug(int id)
    {
        var res = await GetDrugById(id, includeData:true);
        if (!res.IsSucceeded || res.Data == null) return res;
        _ctx.Remove(res.Data);
        await _ctx.SaveChangesAsync();
        return res;
    }

    public async Task<ICollection<Drug>> GetAllDrugs()
    {
        return await _ctx.Drugs
                .Include(d => d.Company)
                .Include(d => d.DrugType)
                .Include(d => d.DrugCategory)
                .Include(d => d.EffectiveMatrials)
                .Where(d=> d.IsTradingPermitted)
                .ToListAsync();
    }

    public async Task<RepoResult<Drug>> GetDrugById(int id , bool includeData)
    {
        Drug? drug; 
        if(!includeData)
        {
            drug = await _ctx.Drugs.SingleOrDefaultAsync(d => d.Id == id);
        }
        else
        {
            drug = await _ctx.Drugs
                .Include(d=>d.Company)
                .Include(d=>d.DrugType)
                .Include(d=>d.DrugCategory)
                .Include(d=>d.EffectiveMatrials)
                .SingleOrDefaultAsync(d => d.Id == id);
        }

        if (drug == null) return _repoResultBuilder.Failuer(new[] { "Drug id is Invalid ,Drug Not Found" });
        return _repoResultBuilder.Success(drug);

    }

    public async  Task<RepoResult<Drug>> UpdateDrug(int id, Drug d)
    {
        var res = await GetDrugById(id, includeData: true);
        if (!res.IsSucceeded || res.Data == null) return res;

        var drug = res.Data;

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
        return _repoResultBuilder.Success(drug);
    }

    
}

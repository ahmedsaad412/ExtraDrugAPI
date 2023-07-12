using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCategoryRepo : IDrugCategoryRepo
{
    private readonly AppDbContext _ctx;
    private readonly RepoResultBuilder<DrugCategory> _repoResultBuilder;

    public DrugCategoryRepo(AppDbContext ctx , RepoResultBuilder<DrugCategory> repoResultBuilder)
    {
        _ctx = ctx;
        _repoResultBuilder = repoResultBuilder;
    }
    public async Task<DrugCategory> AddDrugCategory(DrugCategory drugCategory)
    {
        _ctx.DrugCategories.Add(drugCategory);
        await _ctx.SaveChangesAsync();
        return drugCategory;
    }

    public async Task<RepoResult<DrugCategory>> DeleteDrugCategory(int Id)
    {
        var dc = await _ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null)
            return _repoResultBuilder.Failuer(new[] { "Category Id Invalid , Category Not Found" });
        _ctx.DrugCategories.Remove(dc);
        await _ctx.SaveChangesAsync(); ;
        return _repoResultBuilder.Success(dc);
    }

    public async Task<ICollection<DrugCategory>> GetAllDrugCategories()
    {
        return await _ctx.DrugCategories.ToListAsync();
    }

    public async Task<RepoResult<DrugCategory>> GetCategoryById(int id )
    {
        var cat =  await _ctx.DrugCategories.FindAsync(id);
        if (cat is null)
            return _repoResultBuilder.Failuer(new[] { "Category Id Invalid , Category Not Found" });
        return _repoResultBuilder.Success(cat);
    }

    public async Task<RepoResult<DrugCategory>> UpdateDrugCategory(int Id, DrugCategory drugCategory)
    {
        var dc = await _ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null)
            return _repoResultBuilder.Failuer(new[] { "Category Id Invalid , Category Not Found" });
        dc.Name = drugCategory.Name;
        await _ctx.SaveChangesAsync(); ;
        return _repoResultBuilder.Success(dc);
    }
}

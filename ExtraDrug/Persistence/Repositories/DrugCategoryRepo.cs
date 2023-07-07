using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCategoryRepo : IDrugCategoryRepo
{
    private readonly AppDbContext ctx;
    public DrugCategoryRepo(AppDbContext _ctx)
    {
        ctx = _ctx;
    }
    public async Task<DrugCategory> AddDrugCategory(DrugCategory drugCategory)
    {
        ctx.DrugCategories.Add(drugCategory);
        await ctx.SaveChangesAsync();
        return drugCategory;
    }

    public async Task<DrugCategory?> DeleteDrugCategory(int Id)
    {
        var dc = await ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        ctx.DrugCategories.Remove(dc);
        await ctx.SaveChangesAsync(); ;
        return dc;
    }

    public async Task<ICollection<DrugCategory>> GetAllDrugCategories()
    {
        return await ctx.DrugCategories.ToListAsync();
    }

    public async Task<DrugCategory?> UpdateDrugCategory(int Id, DrugCategory drugCategory)
    {
        var dc = await ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        dc.Name = drugCategory.Name;
        await ctx.SaveChangesAsync(); ;
        return dc;
    }
}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugCategoryRepo : IDrugCategoryRepo
{
    private readonly AppDbContext _ctx;
    public DrugCategoryRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<DrugCategory> AddDrugCategory(DrugCategory drugCategory)
    {
        _ctx.DrugCategories.Add(drugCategory);
        await _ctx.SaveChangesAsync();
        return drugCategory;
    }

    public async Task<DrugCategory?> DeleteDrugCategory(int Id)
    {
        var dc = await _ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        _ctx.DrugCategories.Remove(dc);
        await _ctx.SaveChangesAsync(); ;
        return dc;
    }

    public async Task<ICollection<DrugCategory>> GetAllDrugCategories()
    {
        return await _ctx.DrugCategories.ToListAsync();
    }

    public async Task<DrugCategory?> GetCategoryById(int id )
    {
        return await _ctx.DrugCategories.FindAsync(id);
    }

    public async Task<DrugCategory?> UpdateDrugCategory(int Id, DrugCategory drugCategory)
    {
        var dc = await _ctx.DrugCategories.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dc is null) return null;
        dc.Name = drugCategory.Name;
        await _ctx.SaveChangesAsync(); ;
        return dc;
    }
}

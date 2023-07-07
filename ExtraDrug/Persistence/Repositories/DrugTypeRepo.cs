using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugTypeRepo : IDrugTypeRepo
{
    private readonly AppDbContext ctx;
    public DrugTypeRepo(AppDbContext _ctx)
    {
        ctx = _ctx;
    }
    public async Task<DrugType> AddDrugType(DrugType drugType)
    {
        ctx.DrugTypes.Add(drugType);
        await ctx.SaveChangesAsync();
        return drugType;
    }

    public async Task<DrugType?> DeleteDrugType(int Id)
    {

        var dt = await ctx.DrugTypes.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dt is null) return null;
        ctx.DrugTypes.Remove(dt);
        await ctx.SaveChangesAsync(); ;
        return dt;
    }

    public async Task<ICollection<DrugType>> GetAllDrugType()
    {
        return await ctx.DrugTypes.ToListAsync();
    }

    public async Task<DrugType?> UpdateDrugType(int Id, DrugType drugType)
    {
        var dt = await ctx.DrugTypes.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dt is null) return null;
        dt.Name = drugType.Name;
        await ctx.SaveChangesAsync(); 
        return dt;
    }
}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugTypeRepo : IDrugTypeRepo
{
    private readonly AppDbContext _ctx;
    public DrugTypeRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<DrugType> AddDrugType(DrugType drugType)
    {
        _ctx.DrugTypes.Add(drugType);
        await _ctx.SaveChangesAsync();
        return drugType;
    }

    public async Task<DrugType?> DeleteDrugType(int Id)
    {

        var dt = await _ctx.DrugTypes.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dt is null) return null;
        _ctx.DrugTypes.Remove(dt);
        await _ctx.SaveChangesAsync(); ;
        return dt;
    }

    public async Task<ICollection<DrugType>> GetAllDrugType()
    {
        return await _ctx.DrugTypes.ToListAsync();
    }

    public async Task<DrugType?> GetTypeById(int id)
    {
        return await _ctx.DrugTypes.FindAsync(id);
    }

    public async Task<DrugType?> UpdateDrugType(int Id, DrugType drugType)
    {
        var dt = await _ctx.DrugTypes.SingleOrDefaultAsync(dc => dc.Id == Id);
        if (dt is null) return null;
        dt.Name = drugType.Name;
        await _ctx.SaveChangesAsync(); 
        return dt;
    }
    
}

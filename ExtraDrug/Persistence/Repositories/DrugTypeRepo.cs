using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class DrugTypeRepo : IDrugTypeRepo
{
    private readonly AppDbContext _ctx;
    private readonly RepoResultBuilder<DrugType> _repoResultBuilder;

    public DrugTypeRepo(AppDbContext ctx ,RepoResultBuilder<DrugType> repoResultBuilder)
    {
        _ctx = ctx;
        _repoResultBuilder = repoResultBuilder;
    }
    public async Task<DrugType> AddDrugType(DrugType drugType)
    {
        _ctx.DrugTypes.Add(drugType);
        await _ctx.SaveChangesAsync();
        return drugType;
    }

    public async Task<RepoResult<DrugType>> DeleteDrugType(int Id)
    {

        var res = await GetTypeById(Id);
        if (res.IsSucceeded || res.Data is null) return res;
        _ctx.DrugTypes.Remove(res.Data);
        await _ctx.SaveChangesAsync(); ;
        return res;
    }

    public async Task<ICollection<DrugType>> GetAllDrugType()
    {
        return await _ctx.DrugTypes.ToListAsync();
    }

    public async Task<RepoResult<DrugType>> GetTypeById(int id)
    {
        var type =  await _ctx.DrugTypes.FindAsync(id);
        if (type is null) return _repoResultBuilder.Failuer(new[] { "Type Not Found, Type Id invalid" });
        return _repoResultBuilder.Success(type);
    }

    public async Task<RepoResult<DrugType>> UpdateDrugType(int Id, DrugType drugType)
    {
        var res = await GetTypeById(Id);
        if (!res.IsSucceeded || res.Data is null) return res;
        res.Data.Name = drugType.Name;
        await _ctx.SaveChangesAsync(); 
        return res;
    }
    
}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class EffectiveMatrialRepo : IEffectiveMatrialRepo
{
    private readonly AppDbContext _ctx;

    public EffectiveMatrialRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async  Task<EffectiveMatrial> Add(EffectiveMatrial ef)
    {
        _ctx.EffectiveMatrials.Add(ef);
        await _ctx.SaveChangesAsync();
        return ef;
    }

    public async  Task<EffectiveMatrial?> Delete(int Id)
    {
        var ef = await GetById(Id);
        if (ef is null ) return null;
        _ctx.Remove(ef);
        await _ctx.SaveChangesAsync();
        return ef;
    }

    public async Task<ICollection<EffectiveMatrial>> GetAll()
    {
       return await _ctx.EffectiveMatrials.ToListAsync();
    }

    public async Task<EffectiveMatrial?> GetById(int id)
    {
       return await _ctx.EffectiveMatrials.SingleOrDefaultAsync(ef => ef.Id == id);
    }

    public async Task<EffectiveMatrial?> Update(int Id, EffectiveMatrial ef)
    {
        var ef_from_db = await GetById(Id);
        if (ef_from_db is null) return null;
        ef_from_db.Name = ef.Name;
        await _ctx.SaveChangesAsync();
        return ef_from_db;
    }

}

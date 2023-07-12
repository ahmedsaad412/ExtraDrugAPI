using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class EffectiveMatrialRepo : IEffectiveMatrialRepo
{
    private readonly AppDbContext _ctx;
    private readonly RepoResultBuilder<EffectiveMatrial> _repoResultBuilder;

    public EffectiveMatrialRepo(AppDbContext ctx , RepoResultBuilder<EffectiveMatrial> repoResultBuilder)
    {
        _ctx = ctx;
        _repoResultBuilder = repoResultBuilder;
    }

    public async  Task<EffectiveMatrial> Add(EffectiveMatrial ef)
    {
        _ctx.EffectiveMatrials.Add(ef);
        await _ctx.SaveChangesAsync();
        return ef;
    }

    public async  Task<RepoResult<EffectiveMatrial>> Delete(int Id)
    {
        var res = await GetById(Id);
        if (!res.IsSucceeded || res.Data is null) return res;
        _ctx.Remove(res.Data);
        await _ctx.SaveChangesAsync();
        return res;
    }

    public async Task<ICollection<EffectiveMatrial>> GetAll()
    {
       return await _ctx.EffectiveMatrials.ToListAsync();
    }

    public async Task<RepoResult<EffectiveMatrial>> GetById(int id)
    {
        var efm = await _ctx.EffectiveMatrials.SingleOrDefaultAsync(ef => ef.Id == id);
        if (efm is null) return _repoResultBuilder.Failuer(new[] { "Effectiva Mattrial Not Found" });
        return _repoResultBuilder.Success(efm);
    }

    public async Task<RepoResult<EffectiveMatrial>> Update(int Id, EffectiveMatrial ef)
    {
        var res = await GetById(Id);
        if (!res.IsSucceeded || res.Data is null) return res;
        res.Data.Name = ef.Name;
        await _ctx.SaveChangesAsync();
        return res;
    }

}

using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class EffectiveMatrialRepo : IEffectiveMatrialRepo
{
    private readonly AppDbContext ctx;

    public EffectiveMatrialRepo(AppDbContext _ctx)
    {
        ctx = _ctx;
    }
    public async Task<ICollection<EffectiveMatrial>> GetAll()
    {
       return await  ctx.EffectiveMatrials.ToListAsync();
    }
}

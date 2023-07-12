using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IEffectiveMatrialRepo
{
    Task<ICollection<EffectiveMatrial>> GetAll();

    Task<EffectiveMatrial> Add(EffectiveMatrial ef);
    Task<RepoResult<EffectiveMatrial>> Delete(int Id);
    Task<RepoResult<EffectiveMatrial>> Update(int Id, EffectiveMatrial ef);
    Task<RepoResult<EffectiveMatrial>> GetById(int id);

}

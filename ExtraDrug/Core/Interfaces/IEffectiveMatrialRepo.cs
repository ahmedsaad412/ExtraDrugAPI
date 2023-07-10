using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IEffectiveMatrialRepo
{
    Task<ICollection<EffectiveMatrial>> GetAll();

    Task<EffectiveMatrial> Add(EffectiveMatrial ef);
    Task<EffectiveMatrial?> Delete(int Id);
    Task<EffectiveMatrial?> Update(int Id, EffectiveMatrial ef);
    Task<EffectiveMatrial?> GetById(int id);

}

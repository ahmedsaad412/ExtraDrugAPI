using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IEffectiveMatrialRepo
{
    Task<ICollection<EffectiveMatrial>> GetAll(); 
}

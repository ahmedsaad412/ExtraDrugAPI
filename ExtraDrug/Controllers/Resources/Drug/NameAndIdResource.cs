using ExtraDrug.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.Drug;

public class NameAndIdResource
{

    public int Id { get; set; }
    [Required, MaxLength(100), MinLength(3)]
    public string Name { get; set; } = String.Empty; 

    public T MapToModel<T>() where T : INameAndId , new()
    {
        return new T()
        {
            Id = Id,
            Name = Name,
        };
    }
    public static NameAndIdResource MapToResource<T>(T model) where T : INameAndId
    {
        return new NameAndIdResource()
        {
            Id = model.Id,
            Name = model.Name,
        };
    }
}

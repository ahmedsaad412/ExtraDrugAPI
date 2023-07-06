namespace ExtraDrug.Controllers.Resources;

public class ErrorResponce
{
    public string? Message { get; set; }
    public ICollection<string>? Errors  { get; set; }
}

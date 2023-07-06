namespace ExtraDrug.Controllers.Resources;

public class SuccessResponce<T>
{
    public string? Message { get; set; }
    public T? Data { get; set; }
    public RequestMetaResource? Meta { get; set; }
}

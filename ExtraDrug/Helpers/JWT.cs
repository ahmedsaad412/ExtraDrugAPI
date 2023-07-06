namespace ExtraDrug.Helpers;

public class JWT
{
    public string Key { get; set; } = "this is my custom Secret key for authentication";
    public string? Issuer { get; set; } = "default Issuer";
    public string? Audience { get; set; } = "default Issuer";
    public int DurationInDays { get; set; } = 30;
}

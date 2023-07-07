﻿namespace ExtraDrug.Core.Models;

public class AuthResult
{
    public ICollection<string>? Errors { get; set; }
    public bool IsSucceeded { get; set; } = false;
    public ApplicationUser?  User { get; set; }
    public string? JwtToken { get; set; }
    public DateTime? ExpiresOn { get; set; }
    public ICollection<string>? UserRoles { get; set; }
}

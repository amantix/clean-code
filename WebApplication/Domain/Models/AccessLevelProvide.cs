using Domain.Enums;

namespace Domain.Models;

public class AccessLevelProvide
{
    public string Email { get; set; }
    public AccessLevel Level { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class UserLoginEmail
{
    [Required]
    public string UserEmail { get; set; }
    [Required] public string Password { get; set; }
}
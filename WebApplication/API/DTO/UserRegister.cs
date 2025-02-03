using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class UserRegister
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string UserEmail { get; set; }
    [Required]
    public string Password { get; set; }
}
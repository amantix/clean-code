using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace API.DTO;

public class UserDocument
{
    [Required]
    public string Title { get; set; }
    [Required]
    public AccessType Type { get; set; }
}
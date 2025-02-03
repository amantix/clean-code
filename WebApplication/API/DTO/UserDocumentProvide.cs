using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace API.DTO;

public class UserDocumentProvide
{
    [Required]
    public string Email { get; set; }
    [Required]
    public AccessLevel AccessLevel { get; set; }
}
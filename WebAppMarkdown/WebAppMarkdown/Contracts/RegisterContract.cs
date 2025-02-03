using System.ComponentModel.DataAnnotations;

namespace WebAppMarkdown.Contracts;

public record RegisterContract(
    [Required] string Email,
    [Required] string Username,
    [Required] string Password);
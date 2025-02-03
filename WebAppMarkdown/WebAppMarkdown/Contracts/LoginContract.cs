using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppMarkdown.Contracts;



public record LoginContract(
    [Required]
    string Email,

    [Required]
    string Password
);
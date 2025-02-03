using System.ComponentModel.DataAnnotations;

namespace WebAppMarkdown.Contracts;

public record SaveContract(
    [Required] string Text,
    [Required] string Title);

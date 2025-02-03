using System.ComponentModel.DataAnnotations;

namespace WebAppMarkdown.Contracts;

public record ConvertContract(
    [Required] string Text);
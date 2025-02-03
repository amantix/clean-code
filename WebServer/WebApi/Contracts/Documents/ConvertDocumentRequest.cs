using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Documents;

public record ConvertDocumentRequest(
    [Required] Guid DocumentId,
    [Required] string Content);
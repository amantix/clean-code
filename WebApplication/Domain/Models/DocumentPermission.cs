using Domain.Enums;

namespace Domain.Models;

public class DocumentPermission
{
    public Guid UserId { get; set; }
    public Guid DocumentId { get; set; }
    public AccessLevel AccessLevel { get; set; }
}
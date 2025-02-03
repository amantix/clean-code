namespace Core.Models;

public class DocumentAccess
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DocumentId { get; set; }
    public DocumentAccessRoles Role { get; set; }

    public static DocumentAccess Create(Guid id, Guid userId, Guid documentId, DocumentAccessRoles role) =>
        new()
        {
            Id = id,
            UserId = userId,
            DocumentId = documentId,
            Role = role
        };
}
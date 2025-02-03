using Core;

namespace Persistence.Entities;

public class DocumentAccessEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DocumentId { get; set; }
    
    public DocumentAccessRoles Role { get; set; }
    
    public UserEntity User { get; set; }
    public DocumentEntity Document { get; set; }
}
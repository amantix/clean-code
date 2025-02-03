namespace Persistence.Entities;

public class DocumentPermissionEntity
{
    public Guid DocumentPermissionId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public Guid DocumentId { get; set; }
    public DocumentEntity Document { get; set; } = null!;
    public int AccessLevelId { get; set; }
    public DocumentAccessLevelEntity AccessLevel { get; set; }  = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
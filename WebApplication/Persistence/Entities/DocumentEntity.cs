namespace Persistence.Entities;

public class DocumentEntity
{
    public Guid DocumentId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid AuthorId { get; set; }
    public UserEntity Author { get; set; } = null!;
    public int AccessTypeId { get; set; }
    public DocumentAccessTypeEntity AccessType { get; set; } = null!;
    public ICollection<DocumentPermissionEntity> Permissions { get; set; } = new List<DocumentPermissionEntity>();
}
namespace Persistence.Entities;

public class DocumentAccessLevelEntity
{
    public int DocumentAccessLevelId { get; set; }
    public string LevelName { get; set; } = null!;
    public ICollection<DocumentPermissionEntity> DocumentPermissions { get; set; }
}
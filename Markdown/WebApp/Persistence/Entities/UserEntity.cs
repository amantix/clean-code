namespace Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    
    public List<DocumentEntity> PersonalDocuments { get; set; } = [];
    public List<DocumentAccessEntity> DocumentAccesses { get; set; } = [];
}
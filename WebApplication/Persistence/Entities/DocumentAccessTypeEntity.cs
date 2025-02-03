using System.Collections;

namespace Persistence.Entities;

public class DocumentAccessTypeEntity
{
    public int DocumentAccessTypeId { get; set; }
    public string TypeName { get; set; } = null!;
    public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();
}
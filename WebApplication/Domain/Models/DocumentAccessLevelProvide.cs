using Domain.Enums;

namespace Domain.Models;

public class DocumentAccessLevelProvide
{
    public Guid DocumentId { get; set; }
    public List<AccessLevelProvide>? Provides { get; set; }
}
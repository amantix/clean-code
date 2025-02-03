namespace Core.Models;

public class Document
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreationDateTime { get; set; }

    public static Document Create(Guid id, string name, Guid authorId, string authorName, DateTime creationDateTime) =>
        new()
        {
            Id = id,
            Name = name,
            AuthorId = authorId,
            AuthorName = authorName,
            CreationDateTime = creationDateTime,
        };
}
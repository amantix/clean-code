namespace WebAppMarkdown.Db.Models;

public class DocumentEntity
{
    public Guid Id { get; set; }
    public string MarkdownContent { get; set; }
    public string HtmlContent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserEntity User { get; set; }
    public Guid UserId { get; set; }
}
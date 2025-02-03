namespace WebAppMarkdown.Core.Models;

public class Document
{
    public Guid Id { get; set; }
    public string MarkdownContent { get; set; }
    public string HtmlContent { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
}
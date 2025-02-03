using WebAppMarkdown.Core.Models;

namespace WebAppMarkdown.Models
{
    public class IndexModel
    {
        public Guid Id { get; set; }
        public string MarkdownContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedContent { get; set; } // Optional: You can add additional properties, such as formatted HTML content

        // Constructor for easier initialization
        public IndexModel(Document document)
        {
            Id = document.Id;
            MarkdownContent = document.MarkdownContent;
        }
    }
}
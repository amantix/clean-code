namespace Markdown.Requests
{
    public class DocumentRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublic { get; set; } = false;
        public string Link { get; set; } = "";
    }
}

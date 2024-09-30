using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer
{
    public class MarkdownConverter : IMarkdownConverter
    {
        public string ConvertToHtml(string unprocessedText)
        {
            return unprocessedText;
        }
    }
}

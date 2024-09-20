using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer
{
    public class MarkdownConverter : IMarkdownConverter
    {
        public string ConvertToHtmlFragment(string unprocessedText)
        {
            throw new NotImplementedException();
        }

        string IMarkdownConverter.ConvertToHtml(string unprocessedText)
        {
            throw new NotImplementedException();
        }
    }
}

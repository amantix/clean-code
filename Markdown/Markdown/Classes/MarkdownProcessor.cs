using System.Text;

namespace Markdown;

public class MarkdownProcessor
{
    private IMarkdownParser parser;
    private IMarkdownRenderer renderer;
    public MarkdownProcessor(IMarkdownParser parser,IMarkdownRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }
    public string GetHtml(string markdownText)
    {
        var elements = parser.Parse(markdownText);
        var html = renderer.Render(elements);
        return html;
    }
}
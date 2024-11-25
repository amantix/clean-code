using MDConverter.Renderer;

namespace MDConverter;

public class MarkdownConverter
{
    private readonly List<IMarkdownRenderer> _renderers = new()
    {
        new EscapeSequenceRenderer(),
        new HeaderRenderer(),
        new BoldRenderer(),
        new ItalicRenderer()
    };

    public string ConvertToHtml(string markdown)
    {
        string result = markdown;
        foreach (var renderer in _renderers)
        {
            result = renderer.Render(result);
        }
        return result;
    }
}
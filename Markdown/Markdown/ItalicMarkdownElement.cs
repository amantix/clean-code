namespace Markdown;

public class ItalicMarkdownElement : IMarkdownElement
{
    private string text;
    public ItalicMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"<em>{text}</em>";
    }
}
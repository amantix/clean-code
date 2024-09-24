namespace Markdown;

public class ItalicMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<em>";
    private string closingTag = "</em>";
    public ItalicMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"{openingTag}{text}{closingTag}";
    }
}
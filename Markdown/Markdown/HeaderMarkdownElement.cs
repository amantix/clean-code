namespace Markdown;

public class HeaderMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<h1>";
    private string closingTag = "</h1>";
    public HeaderMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"{openingTag}{text}{closingTag}";
    }
}
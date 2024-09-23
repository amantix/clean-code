namespace Markdown;

public class StrongMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<strong>";
    private string closingTag = "</strong>";
    public StrongMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"{openingTag}{text}{closingTag}";
    }
}
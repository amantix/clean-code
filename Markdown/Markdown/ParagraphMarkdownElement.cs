namespace Markdown;

public class ParagraphMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<p>";
    private string closingTag = "</p>";
    public ParagraphMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"{openingTag}{text}{closingTag}";
    }
}
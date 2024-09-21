namespace Markdown;

public class HeaderMarkdownElement : IMarkdownElement
{
    private string text;
    public HeaderMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"<h1>{text}</h1>";
    }
}
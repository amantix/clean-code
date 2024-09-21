namespace Markdown;

public class StrongMarkdownElement : IMarkdownElement
{
    private string text;
    public StrongMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"<h1>{text}</h1>";
    }
}
namespace Markdown;

public class StrongMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<strong>";
    private string closingTag = "</strong>";
    public StrongMarkdownElement(string line)
    {
        text = line.Substring(2,line.Length-4);
    }
    public string GetHtmlLine()
    {
        return $"{openingTag}{text}{closingTag}";
    }
}
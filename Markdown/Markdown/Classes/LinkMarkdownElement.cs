namespace Markdown;

public class LinkMarkdownElement : IMarkdownElement
{
    private string link;
    private string text;

    public LinkMarkdownElement(string line)
    {
        var split = line.Split("](");
        link = split[1].Substring(0,split[1].Length-1);
        text = split[0].Substring(1);
    }
    public string GetHtmlLine()
    {
        return $"<a href='{link}'>{text}</a>";
    }
}
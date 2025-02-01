using System.Text;
using Markdown;

public class HeaderMarkdownElement : IMarkdownElement
{
    private string text;
    private string openingTag = "<h1>";
    private string closingTag = "</h1>";
    public HeaderMarkdownElement(string line)
    {
        text = line.Substring(1);
    }
    public string GetHtmlLine()
    {
        var nestedText = ProcessNested(text);
        return $"{openingTag}{nestedText}{closingTag}\n";
    }

    private string ProcessNested(string text)
    {
        var nestedTextProcessor = new NestedTextProcessor(text,TypeOfElement.HeaderMarkdownElement);
        var result = nestedTextProcessor.GetNestedHtmlLine();
        return result;
    }
}

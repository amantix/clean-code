using System.Text;

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
        var nestedText = ProcessNested(text);
        return $"{openingTag}{nestedText}{closingTag}\n";
    }
    private string ProcessNested(string text)
    {
        var nestedTextProcessor = new NestedTextProcessor(text,TypeOfElement.ParagraphMarkdownElement);
        var result = nestedTextProcessor.GetNestedHtmlLine();
        return result;
    }
}
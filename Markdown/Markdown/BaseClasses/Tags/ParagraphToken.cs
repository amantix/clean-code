using Markdown.AbstractClasses;

namespace Markdown.Tags;

public class ParagraphToken: BaseMarkdownToken
{
    public override string ToHtml()
    {
        var htmlResultString = string.Join("", Children.Select(child => child.ToHtml()));
        return "<p>" + htmlResultString + "</p>";
    }
}
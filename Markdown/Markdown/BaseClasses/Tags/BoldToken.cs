using Markdown.AbstractClasses;

namespace Markdown.Tags;

public class BoldToken: BaseMarkdownToken
{
    //public string Content { get; }
    //public BoldToken(string content)
    //{
    //    Content = content;
    //}
    public override string ToHtml()
    {
        var htmlResultString = string.Join("", Children.Select(child => child.ToHtml()));
        return ("<strong>" + htmlResultString + "</strong>");
    }
}
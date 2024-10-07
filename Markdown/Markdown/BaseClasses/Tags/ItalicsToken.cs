using Markdown.AbstractClasses;

namespace Markdown.Tags;

public class ItalicsToken: BaseMarkdownToken
{
    //public string Content { get; }
    //public ItalicsToken(string content)
    //{
    //    Content = content;
    //}
    public override string ToHtml()
    {
        var htmlResultString = string.Join("", Children.Select(child => child.ToHtml()));
        return ("<em>" + htmlResultString + "</em>");
    }
}
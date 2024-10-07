using System.Xml.Linq;

namespace Markdown.AbstractClasses;

public abstract class BaseMarkdownToken
{
    public List<BaseMarkdownToken> Children { get; } = new List<BaseMarkdownToken>();
    public void AddChild(BaseMarkdownToken child)
    {
        Children.Add(child);
    }

    public abstract string ToHtml();
}
using MDprocessor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

public class HtmlRenderer : IHtmlRenderer
{
    public string Render(List<IMarkdownElement> elements)
    {
        return RenderElements(elements);
    }

    private string RenderElements(IEnumerable<IMarkdownElement> elements)
    {
        var sb = new StringBuilder();
        foreach (var element in elements)
        {
            sb.Append(RenderElement(element));
        }
        return sb.ToString();
    }

    private string RenderElement(IMarkdownElement element)
    {
        switch (element)
        {
            case HeadingElement heading:
                // Ограничим уровень 1..6
                int level = heading.Level;
                if (level < 1) level = 1;
                if (level > 6) level = 6;
                return $"<h{level}>{RenderElements(heading.Children)}</h{level}>";

            case StrongElement strong:
                return $"<strong>{RenderElements(strong.Children)}</strong>";

            case EmphasisElement em:
                return $"<em>{RenderElements(em.Children)}</em>";

            case TextElement text:
                return EscapeHtml(text.Text);

            default:
                return string.Empty;
        }
    }
    private string EscapeHtml(string text)
    {
        return System.Net.WebUtility.HtmlEncode(text);
    }
}



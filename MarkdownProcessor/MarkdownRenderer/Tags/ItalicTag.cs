using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Tags
{
    public class ItalicTag: Tag
    {
        public override string MarkdownSymbol => "_";
        public override string HtmlTag => "em";
    }
}

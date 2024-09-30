using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Tags
{
    public class BoldTag : Tag
    {
        public override string MarkdownSymbol => "__";

        public override string HtmlTag => "strong";
    }
}

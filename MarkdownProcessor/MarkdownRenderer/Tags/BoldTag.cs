using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Tags
{
    public class BoldTag : Tag
    {
        public override string MarkdownSymbol => "**";

        public override string HtmlTag => "b";
    }
}

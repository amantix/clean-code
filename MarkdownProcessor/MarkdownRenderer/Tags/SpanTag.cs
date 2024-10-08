using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer.Tags;

public class SpanTag: Tag
{
    public override string MarkdownSymbol => string.Empty;
    public override string HtmlTag => "span";
    public override TagType TagType => TagType.SpanTag;
}
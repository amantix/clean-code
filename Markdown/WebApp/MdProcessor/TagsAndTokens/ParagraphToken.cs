using MdProcessor.Abstractions;
using MdProcessor.Enums;

namespace MdProcessor.TagsAndTokens;

public class ParagraphToken : TagToken
{
    public override string HtmlTag => "p";
    public override string MarkdownTag => "paragraph";
    public override TagType Type => TagType.Paragraph;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }

    public override object Clone() => new ParagraphToken()
    {
        Pair = (TagToken?)Pair?.Clone(),
    };
}
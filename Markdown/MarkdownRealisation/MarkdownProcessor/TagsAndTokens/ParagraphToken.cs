using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

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
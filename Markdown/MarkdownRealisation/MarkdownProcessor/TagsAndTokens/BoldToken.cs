using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class BoldToken : TagToken
{
    public override string HtmlTag => "strong";
    public override string MarkdownTag => "__";
    public override TagType Type => TagType.Bold;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }

    public override object Clone() => new BoldToken()
    {
        Pair = (TagToken?)Pair?.Clone(),
    };
}
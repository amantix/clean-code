using MarkdownRealisation.Abstractions;
using MarkdownRealisation.Enums;

namespace MarkdownRealisation.TagsAndTokens;

public class ItalicToken : TagToken
{
    public override string HtmlTag => "em";
    public override string MarkdownTag => "_";
    public override TagType Type => TagType.Italic;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }

    public override object Clone() => new ItalicToken()
    {
        Pair = (TagToken?)Pair?.Clone()
    };
}
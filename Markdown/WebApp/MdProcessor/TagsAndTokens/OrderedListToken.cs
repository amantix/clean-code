using MdProcessor.Abstractions;
using MdProcessor.Enums;

namespace MdProcessor.TagsAndTokens;

public class OrderedListToken : TagToken
{
    public override string HtmlTag => "ol";
    public override string MarkdownTag => "";
    public override TagType Type => TagType.OrderedList;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }
    public override object Clone() => new OrderedListToken()
    {
        Pair = (TagToken?)Pair?.Clone()
    };
}
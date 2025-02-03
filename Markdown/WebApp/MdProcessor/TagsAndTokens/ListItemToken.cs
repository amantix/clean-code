using MdProcessor.Abstractions;
using MdProcessor.Enums;

namespace MdProcessor.TagsAndTokens;

public class ListItemToken(int number) : ListToken(number)
{
    public override int Offset { get; } = number;
    
    public override string HtmlTag => "li";
    public override string MarkdownTag => "";
    public override TagType Type => TagType.ListItem;
    public override TagPosition Position { get; set; }
    public override TagToken? Pair { get; set; }

    public override object Clone() => new ListItemToken(Offset)
    {
        Pair = (TagToken?)Pair?.Clone()
    };

}
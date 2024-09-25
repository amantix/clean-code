using Markdown.Enums;

namespace Markdown.Classes;

public class Token
{
    public uint StartIndex { get; set; }
    public uint EndIndex { get; set; }
    public Style Style { get; set; }
}

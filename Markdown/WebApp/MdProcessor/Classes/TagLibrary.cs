using MdProcessor.Abstractions;
using MdProcessor.TagsAndTokens;

namespace MdProcessor.Classes;

public static class TagLibrary
{
    public static readonly string[] Tags = ["__", "_"];

    public static TagToken GetTagToken(string mdTag)
    {
        return mdTag switch
        {
            "######" => new HeaderToken(6),
            "#####"  => new HeaderToken(5),
            "####"   => new HeaderToken(4),
            "###"    => new HeaderToken(3),
            "##"     => new HeaderToken(2),
            "#"      => new HeaderToken(1),
            "__"     => new BoldToken(),
            "_"      => new ItalicToken(),
            _        => throw new Exception($"Unknown tag: {mdTag}")
        };
    }
}
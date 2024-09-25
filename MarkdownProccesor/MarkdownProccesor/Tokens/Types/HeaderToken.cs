using MarkdownProccesor.Tokens.Abstract;

namespace MarkdownProccesor.Tokens.Types;

public sealed class HeaderToken : Token
{
    private uint _levelOfHeader;

    public override TokenType TypeOfToken => TokenType.Header;
    public override string Value => "#";
    public override string TagValue => _isOpeningTag ? $"<h{_levelOfHeader}>" : $"</h{_levelOfHeader}>";

    public HeaderToken(uint level, bool isOpenTag)
    {
        _levelOfHeader = level;
        _isOpeningTag = isOpenTag;
    }
}

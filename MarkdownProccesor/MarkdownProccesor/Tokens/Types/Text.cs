using MarkdownProccesor.Tokens.Abstract;

namespace MarkdownProccesor.Tokens.Types;

public sealed class TextToken : Token
{
    public override TokenType TypeOfToken => TokenType.Text;
    public override string? Value { get; }
    public override string? TagValue => Value;

    public TextToken(string value)
    {
        Value = value;
    }
    public TextToken()
    {

    }
}

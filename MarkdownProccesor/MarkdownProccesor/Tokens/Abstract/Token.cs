namespace MarkdownProccesor.Tokens.Abstract;

public abstract class Token
{
    protected bool _isOpeningTag;
    public abstract TokenType TypeOfToken { get; }
    public abstract string? Value { get; }
    public abstract string? TagValue { get; }

}


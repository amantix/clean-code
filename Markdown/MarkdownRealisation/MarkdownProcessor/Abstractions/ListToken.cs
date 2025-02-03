namespace MarkdownRealisation.Abstractions;

public abstract class ListToken(int offset) : TagToken
{
    public abstract int Offset { get; }
}
using MarkdownRealisation.Abstractions;

namespace MarkdownRealisation.TagsAndTokens
{
    public class TextToken(string content) : Token
    {
        public override string ToString() => content;
    }
}
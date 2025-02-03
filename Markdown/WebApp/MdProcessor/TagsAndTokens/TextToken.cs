using MdProcessor.Abstractions;

namespace MdProcessor.TagsAndTokens
{
    public class TextToken(string content) : Token
    {
        public override string ToString() => content;
    }
}
namespace Markdown;

public interface IMarkdownParser
{
    public List<MdToken> ParseTokens(string input);
}
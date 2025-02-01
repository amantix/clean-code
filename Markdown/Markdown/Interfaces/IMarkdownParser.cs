namespace Markdown;

public interface IMarkdownParser
{
    public List<IMarkdownElement> Parse(string markdownText);
}
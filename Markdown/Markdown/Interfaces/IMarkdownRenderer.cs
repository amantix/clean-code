namespace Markdown;

public interface IMarkdownRenderer
{
    public string Render(List<IMarkdownElement> elements);
}
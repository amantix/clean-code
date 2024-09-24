using Markdown.Classes;

namespace Markdown.Interfaces;

public interface IRenderer
{
    public string Render(List<Token> text);
}
using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class ConsoleMdRenderer: IRenderer
{
    public bool RenderMarkdown(List<TokenType> tokens)
    {
        // Здесь как-то выводим результат в консоль
        throw new NotImplementedException();
    }
}
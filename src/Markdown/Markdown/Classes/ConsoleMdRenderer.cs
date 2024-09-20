using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class ConsoleMdRenderer: IRenderer
{
    public string RenderedMdText { get; private set; }
    
    public bool RenderMarkdown(List<TokenType> tokens)
    {
        // Заглушка для тестирования
        return true;
        // Здесь как-то выводим результат в консоль
        throw new NotImplementedException();
    }
}
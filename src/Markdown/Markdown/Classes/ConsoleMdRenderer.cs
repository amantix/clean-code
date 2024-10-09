using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class ConsoleMdRenderer: IRenderer
{
    public string RenderMarkdown(List<TokenType> tokens)
    {
        // Заглушка для тестирования
        return string.Empty;
        // Здесь как-то выводим результат в консоль
        throw new NotImplementedException();
    }
}
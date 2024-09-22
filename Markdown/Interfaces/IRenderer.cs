using Markdown.Classes;

namespace Markdown.Interfaces;

public interface IRenderer
{
    /// <summary>
    /// Рендерить из токенов HTML-код
    /// </summary>
    /// <param name="tokens">Список токенов для составления HTML-кода</param>
    /// <param name="outputLine">HTML-код</param>
    /// <returns>Удалось ли отрендерить список токенов</returns>
    bool Render(List<Token> tokens, out string outputLine);
}

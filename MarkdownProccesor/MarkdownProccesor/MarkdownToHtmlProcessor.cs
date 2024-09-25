using MarkdownProccesor.Lexer;
using MarkdownProccesor.Renderer;
using MarkdownProccesor.Lexer.Abstract;
using MarkdownProccesor.Renderer.Abstract;
using MarkdownProccesor.Tokens.Abstract;

namespace MarkdownProccesor;

/// <summary>
/// Класс, представляющий собой markdown процессор
/// </summary>
public sealed class MarkdownToHtmlProcessor
{
    /// <summary>
    /// Метод конвертирует markdown в html
    /// Здесь можно подставить реализации интерфейсов со своими алгоритмами
    /// </summary>
    /// <typeparam name="T">коллекция токенов</typeparam>
    /// <param name="markdwontext">текст markdown разметки</param>
    /// <param name="lexer">абстракция лексера </param>
    /// <param name="renderer">абстракция рендерера</param>
    /// <returns>строка html</returns>
    public string ConvertToHtml<T>(string markdwontext, ILexer<T> lexer, IMarkdownRenderer<T> renderer) 
        where T : ICollection<Token>
    {
        return renderer.RenderMarkdwonText(lexer.Tokenize(markdwontext));
    }
    /// <summary>
    /// перегруженный метод, когда нам важен просто результат в виде строки html
    /// </summary>
    /// <param name="markdwontext">строка markdown </param>
    /// <returns>строка html</returns>
    public string ConvertToHtml(string markdwontext)
    {
        return ConvertToHtml(markdwontext, new MarkdownLexer(), new MarkdownToHtmlRenderer());
    }
}

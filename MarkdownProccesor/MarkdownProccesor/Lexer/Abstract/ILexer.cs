using MarkdownProccesor.Tokens.Abstract;

namespace MarkdownProccesor.Lexer.Abstract;

/// <summary>
/// Интерфейс отвечает за разбитие markdwon разметки на токены
/// </summary>
/// <typeparam name="T">Коллекция токенов</typeparam>
public interface ILexer<out T> where T : ICollection<Token>
{
    /// <summary>
    /// метод токенизации
    /// </summary>
    /// <param name="markdownText">текст markdown</param>
    /// <returns>Коллекция токенов(можно реализовать разными алгоритмами)</returns>
    T Tokenize(string markdownText);
}

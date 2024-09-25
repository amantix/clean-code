using MarkdownProccesor.Tokens.Abstract;

namespace MarkdownProccesor.Renderer.Abstract;

/// <summary>
/// интерфейс отображения токенов в текст
/// </summary>
/// <typeparam name="T">коллекция токенов</typeparam>
public interface IMarkdownRenderer<in T> where T : ICollection<Token>
{
    /// <summary>
    /// метод рендера токенов
    /// </summary>
    /// <param name="tokens">коллекция токенов</param>
    /// <returns>строка html</returns>
    string RenderMarkdwonText(T tokens);
}
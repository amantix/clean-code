using MdProcessor.Abstractions;

namespace MdProcessor.Interfaces;

public interface IRender
{
    /// <summary>
    /// Конвертирует токены в другую разметку
    /// </summary>
    /// <param name="tokens">Токены</param>
    /// <returns>Текст с разметкой</returns>
    public string RenderTokens(Token[] tokens);
}
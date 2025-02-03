using MarkdownRealisation.Abstractions;

namespace MarkdownRealisation.Interfaces;

public interface IParser
{
    /// <summary>
    /// Приводит исходный массив строк в массив массивов токенов, по массиву на каждую строчку
    /// </summary>
    /// <param name="text">Массив строк .md файла</param>
    /// <returns>Массив массивов токенов</returns>
    public Token[] Parse(string text);
}
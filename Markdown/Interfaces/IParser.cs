using Markdown.Classes;

namespace Markdown.Interfaces;

public interface IParser
{
    /// <summary>
    /// Метод для парса строк из MD файла в токены 
    /// </summary>
    /// <param name="textOfFile">текст файла или путь к нему</param>
    /// <param name="tokens">список токенов, в который будут записываться полученные результаты</param>
    /// <returns>Получилось ли запарсить текст</returns>
    bool Parse(string textOfFile, List<Token> tokens);
}

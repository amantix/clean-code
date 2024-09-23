using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

// Парсит строку и придает желательно ей какой-то рабочий вид
public class StringParser: IParser
{
    // В контексте данного класса строка textToBeMarkdown - строка, которую нужно превратить в html
    public bool TryParse(string textToBeMarkdown, List<TokenType> tokens)
    {
        // Заглушка для тестирования
        return true;
        throw new NotImplementedException();
    }
}
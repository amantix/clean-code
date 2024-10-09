using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class FileParser: IParser
{
    // В контексте данного класса строка textToBeMarkdown - путь к файлу .md, который нужно запарсить
    public bool TryParse(string textToBeMarkdown, List<TokenType> tokens)
    {
        throw new NotImplementedException();
    }
}
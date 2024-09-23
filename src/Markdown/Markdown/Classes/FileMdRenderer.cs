using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class FileMdRenderer: IRenderer
{
    public readonly string PathToFileToWrite;
    
    public FileMdRenderer(string pathToFileToWrite)
    {
        PathToFileToWrite = pathToFileToWrite;
    }
    
    public string RenderMarkdown(List<TokenType> tokens)
    {   // Здесь как-то записываем html в файлик по директории PathToFile
        throw new NotImplementedException();
    }
}
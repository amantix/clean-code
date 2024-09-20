using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class FileMdRenderer: IRenderer
{
    public readonly string PathToFile;
    public string RenderedMdText { get; private set; }
    
    public FileMdRenderer(string pathToFile)
    {
        PathToFile = pathToFile;
    }
    
    public bool RenderMarkdown(List<TokenType> tokens)
    {   // Здесь как-то записываем html в файлик по директории PathToFile
        throw new NotImplementedException();
    }
}
using Markdown.Classes;
using Markdown.Interfaces;

namespace Markdown;

public class MdProcessor
{
    private IRenderer _renderer = new Renderer();
    private IParser _parser = new Parser();
    static void Main(string[] args)
    {
        
    }
    
}

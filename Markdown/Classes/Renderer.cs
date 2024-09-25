using Markdown.Interfaces;

namespace Markdown.Classes;

public class Renderer : IRenderer
{
    public bool TryRender(List<Token> tokens, out string outputLine)
    {
        // Временные действия для проверки теста
        outputLine = "";
        return true;
        throw new NotImplementedException();
    }
}

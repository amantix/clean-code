using Markdown.Classes;

namespace Markdown.Interfaces
{
    public interface IRenderer
    {
        public string RenderHTML(List<Token> tokens);
    }
}

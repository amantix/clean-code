using Markdown.Classes;
using Markdown.Interfaces;

namespace Markdown
{
    public class MDProcessor
    {
        private IRenderer renderer = new Renderer();
        private IParser parser = new Parser();

        public string MdToHTML(string text)
        {
            List<Token> tokens = parser.ParseMD(text);
            return renderer.RenderHTML(tokens);
        }
    }
}

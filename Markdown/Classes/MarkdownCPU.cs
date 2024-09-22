using Markdown.Interfaces;

namespace Markdown.Classes;

public class MarkdownCPU
{
    private IParser parser;
    private IRenderer renderer;

    public MarkdownCPU(IParser parser, IRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    // Вместо text можно будет указать путь файлу и работать с ним напрямик
    public string ConvertToHTML(string text)
    {
        var tmpList = new List<Token>();
        parser.Parse(text, tmpList);
        renderer.Render(tmpList, out string outputLine);

        return "<h1>header</h1>";
    }
}

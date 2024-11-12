using MarkdownProcessorLib.Services;


namespace MarkdownProcessorLib;

public class Md
{
    public string Render(string text)
    {
        var blocks = new BlockSplitter().Split(text);
        var resultHtml = new HtmlBuilder().Build(blocks);

        return resultHtml;
    }
}

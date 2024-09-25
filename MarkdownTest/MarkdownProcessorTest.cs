using Markdown.Classes;

namespace MarkdownTest;

public class MarkdownProcessorTest
{
    [Fact]
    public void Test1()
    {
        var parser = new Parser();
        var renderer = new Renderer();
        var mdCPU = new MarkdownProcessor(parser, renderer);
        var inputLine = "#header";
        var outputLine = "<h1>header</h1>";

        string result = mdCPU.ConvertToHTML(inputLine);

        Assert.Equal(outputLine, result);
    }
}
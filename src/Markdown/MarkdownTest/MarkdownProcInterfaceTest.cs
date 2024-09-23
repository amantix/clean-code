using Markdown;
using Markdown.Classes;
using Markdown.Interfaces;

namespace MarkdownTest;

public class MarkdownProcInterfaceTest
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var parser = new StringParser();
        var renderer = new ConsoleMdRenderer();
        var mdProc = new MdProcessor(parser, renderer);
        var input = "__text__";
        var expectedOutput = "<b>text</b>";

        // Act
        string result = mdProc.ParseAndRender(input);
        

        // Assert
        Assert.Equal(expectedOutput, result);
    }
}
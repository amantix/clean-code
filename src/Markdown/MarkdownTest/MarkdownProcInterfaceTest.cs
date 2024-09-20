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
        var parser = new TextParser();
        var renderer = new ConsoleMdRenderer();
        var mdProc = new Md(parser, renderer);
        var input = "__text__";
        var expectedOutput = "<b>text</b>";

        // Act
        string result = mdProc.ParseAndRender(input);
        

        // Assert
        Assert.Equal(expectedOutput, result);
    }
}
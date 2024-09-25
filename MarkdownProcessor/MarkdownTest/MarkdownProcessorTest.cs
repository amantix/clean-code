using Markdown.Classes;

namespace MarkdownTest;

public class MarkdownProcessorTest
{
    [Fact]
    public void ConvertToHtml_Should_Handle_Headers()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#Headline";
        var expectedHtml = "<h1>Headline</h1>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Bold_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "**Bold text**";
        var expectedHtml = "<strong>Bold text</strong>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Italic_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "*Italic text*";
        var expectedHtml = "<em>Italic text</em>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Line_Breaks()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "line 1\n Line 2";
        var expectedHtml = "Line 1<br> Line 2";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }
}
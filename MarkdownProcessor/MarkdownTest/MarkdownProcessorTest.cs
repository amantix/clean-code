using Markdown.Classes;

namespace MarkdownTest;

public class MarkdownProcessorTest
{
    [Fact]
    public void ConvertToHtml_Should_Handle_Headers()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "#Заголовок";
        var expectedHtml = "<h1>Заголовок</h1>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Bold_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "**Жирный текст**";
        var expectedHtml = "<strong>Жирный текст</strong>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Italic_Text()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "*Курсивный текст*";
        var expectedHtml = "<em>Курсивный текст</em>";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }

    [Fact]
    public void ConvertToHtml_Should_Handle_Line_Breaks()
    {
        var markdownProcessor = new MarkdownProcessor();
        var markdownText = "Строка 1\n Строка 2";
        var expectedHtml = "Строка 1<br> Строка 2";
        var actualHtml = markdownProcessor.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, actualHtml);
    }
}
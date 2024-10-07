namespace MarkdownTests;
using Markdown;

public class MarkdownToHtmlTests
{
    [Fact]
    public void Render_CursiveText_ReturnHtmlEmTag()
    {
        var processor = new MarkdownToHtmlBackup();
        string markdownText = "_Проверка текста с заменой на em_test@_";
        string expectedText = "<em>Проверка текста с заменой на em</em>test@_";
        var result = processor.Render(markdownText);
        Assert.Equal(expectedText, result);
    }
}
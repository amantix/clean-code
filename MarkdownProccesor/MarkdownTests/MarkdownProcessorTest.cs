using MarkdownProccesor;

namespace MarkdownTests;

public class MarkdownProcessorTest
{
    [Fact]
    public void ConvertToHtmlTagShouldBeMatchHeaderTwoLevel()
    {
        //Arrange
        var mdProcessor = new MarkdownToHtmlProcessor();
        string mdTextInput = "## Заголовок";
        string htmlTextExpected = "<h2>Заголовок</h2>";
        //Act
        string result = mdProcessor.ConvertToHtml(mdTextInput);
        //Assert
        Assert.Equal(htmlTextExpected, result);
    }
}
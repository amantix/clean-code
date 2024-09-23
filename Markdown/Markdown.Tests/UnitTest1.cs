namespace Markdown.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void HeaderMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "My string";
        var expectedHtml = "<h1>My string</h1>";
        var element = new HeaderMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [Test]
    public void ItalicMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "My string";
        var expectedHtml = "<em>My string</em>";
        var element = new ItalicMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [Test]
    public void StrongMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "My string";
        var expectedHtml = "<strong>My string</strong>";
        var element = new StrongMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [Test]
    public void ParagraphMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString()
    {
        // Arrange
        var text = "My string";
        var expectedHtml = "<p>My string</p>";
        var element = new ParagraphMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }
}
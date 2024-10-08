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
        var text = "#My string";
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
        var text = "_My string_";
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
        var text = "__My string__";
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
    
    [TestCase("#My string","<h1>My string</h1>\n")]
    [TestCase("_My String_","<em>My String</em>\n")]
    [TestCase("__My string__","<strong>My string</strong>\n")]
    [TestCase("My string","<p>My string</p>\n")]
    [TestCase("#My string\n_My string_\n__My string__",
        "<h1>My string</h1>\n<em>My string</em>\n<strong>My string</strong>\n")]
    [TestCase("#Заголовок с _курсивом_","<h1>Заголовок с <em>курсивом</em></h1>")]
    public void MarkdownProcessor_GetHtml_ShouldReturnCorrectHtml(string markdownText, string expectedHtml)
    {
        // Arrange
        var markdownProcessor = new MarkdownProcessor();
        // Act
        var html = markdownProcessor.GetHtml(markdownText);
        // Assert
        Assert.AreEqual(expectedHtml,html);
    }
}
using System.Net.Mime;

namespace Markdown.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("#Заголовок с _курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным__", "<h1>Заголовок с <strong>жирным</strong></h1>\n")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным _жирным_ текстом__",
        "<h1>Заголовок с <strong>жирным <em>жирным</em> текстом</strong></h1>\n")]
    [TestCase("#Заголовок с _жирным __жирным__ текстом_",
        "<h1>Заголовок с <em>жирным __жирным__ текстом</em></h1>\n")]
    [TestCase("#This is a \\_simple\\_ text.", "<h1>This is a _simple_ text.</h1>\n")]
    [TestCase("#This is a \\\\_simple_ text.", "<h1>This is a \\<em>simple</em> text.</h1>\n")]
    [TestCase("#This is a sim\\ple text.", "<h1>This is a sim\\ple text.</h1>\n")]
    [TestCase("#This is a \\_simple __strong__ \\_ text.", "<h1>This is a _simple <strong>strong</strong> _ text.</h1>\n")]
    [TestCase("#This is a \\__simple __strong__ \\__ text.", "<h1>This is a _<em>simple __strong__ _</em> text.</h1>\n")]
    [TestCase("#This is a number _12_3 and should not be italic.", "<h1>This is a number _12_3 and should not be italic.</h1>\n")]
    [TestCase("#Text _ italics_ here.", "<h1>Text _ italics_ here.</h1>\n")]
    [TestCase("#This _italic _ text jumps.", "<h1>This _italic _ text jumps.</h1>\n")]
    public void HeaderMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text,string expectedHtml)
    {
        // Arrange
        var element = new HeaderMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }

    [TestCase("_This is italic text._", "<em>This is italic text.</em>\n")]
    [TestCase("_This is __nested strong__ text._", "<em>This is __nested strong__ text.</em>\n")]
    [TestCase("_This is a _nested italic_ text._", "<em>This is a _nested italic_ text.</em>\n")]
    [TestCase("_Number _12_3 should not be italic._", "<em>Number _12_3 should not be italic.</em>\n")]
    [TestCase("_This text ends with _italics__.", "<em>This text ends with _italics__</em>\n")]
    [TestCase("_Text _italics_ jumps._", "<em>Text _italics_ jumps.</em>\n")]
    [TestCase("_This is a sim\\ple text._", "<em>This is a sim\\ple text.</em>\n")]
    [TestCase("_This is __strong _nested__ text._", "<em>This is __strong _nested__ text.</em>\n")]
    [TestCase("_Empty underscores _ not italic_", "<em>Empty underscores _ not italic</em>\n")]
    public void ItalicMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text, string expectedHtml)
    {
        // Arrange
        var element = new ItalicMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }


    [TestCase("__This is a simple text.__", "<strong>This is a simple text.</strong>\n")]
    [TestCase("__This is _italic_ text.__", "<strong>This is <em>italic</em> text.</strong>\n")]
    [TestCase("__This is a number _12_3 and should not be italic.__", "<strong>This is a number _12_3 and should not be italic.</strong>\n")]
    [TestCase("___start_ and _middle_ and _end_.__", "<strong><em>start</em> and <em>middle</em> and <em>end</em>.</strong>\n")]
    [TestCase("__This _is_ a simple _test_.__", "<strong>This <em>is</em> a simple <em>test</em>.</strong>\n")]
    [TestCase("__This is _italic text.__", "<strong>This is _italic text.</strong>\n")]
    [TestCase("__This is _italic _nested_ text_.__", "<strong>This is <em>italic _nested</em> text_.</strong>\n")]
    [TestCase("__Text _ italics_ here.__", "<strong>Text _ italics_ here.</strong>\n")]
    [TestCase("__This _italic _ text jumps.__", "<strong>This _italic _ text jumps.</strong>\n")]
    [TestCase("__This is a \\_simple\\_ text.__", "<strong>This is a _simple_ text.</strong>\n")]
    [TestCase("__This is a \\\\_simple_ text.__", "<strong>This is a \\<em>simple</em> text.</strong>\n")]
    [TestCase("__This is a sim\\ple text.__", "<strong>This is a sim\\ple text.</strong>\n")]
    public void StrongMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text,string expectedHtml)
    {
        // Arrange
        var element = new StrongMarkdownElement(text);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }
    
    [TestCase("[google](https://www.google.ru/?hl=ru)", "<a href='https://www.google.ru/?hl=ru'>google</a>")]
    [TestCase("[](/someurl)", "<a href='/someurl'></a>")]
    [TestCase("[Link]()", "<a href=''>Link</a>")]
    [TestCase("[Link](https://example.com/?a=1&b=2)", "<a href='https://example.com/?a=1&b=2'>Link</a>")]
    [TestCase("[<b>bold</b>](https://example.com)", "<a href='https://example.com'><b>bold</b></a>")]
    [TestCase("[Absolute Link](https://example.com)", "<a href='https://example.com'>Absolute Link</a>")]
    [TestCase("[Relative Link](/home)", "<a href='/home'>Relative Link</a>")]
    public void LinkMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string markdown, string expectedHtml)
    {
        // Arrange
        var element = new LinkMarkdownElement(markdown);

        // Act
        var htmlLine = element.GetHtmlLine();

        // Assert
        Assert.AreEqual(expectedHtml, htmlLine);
    }


    [TestCase("This is a simple paragraph.", "<p>This is a simple paragraph.</p>\n")]
    [TestCase("Paragraph with _italic_ text.", "<p>Paragraph with <em>italic</em> text.</p>\n")]
    [TestCase("Paragraph with __bold__ text.", "<p>Paragraph with <strong>bold</strong> text.</p>\n")]
    [TestCase("Paragraph with __bold and _italic_ text__.", "<p>Paragraph with <strong>bold and <em>italic</em> text</strong>.</p>\n")]
    [TestCase("Paragraph with escaped \\_italic\\_.", "<p>Paragraph with escaped _italic_.</p>\n")]
    [TestCase("Paragraph with multiple lines\nand more content.", "<p>Paragraph with multiple lines\nand more content.</p>\n")]
    [TestCase("This is a simple \\ paragraph.", "<p>This is a simple \\ paragraph.</p>\n")]
    [TestCase("Paragraph with number 12_3, not italic.", "<p>Paragraph with number 12_3, not italic.</p>\n")]
    [TestCase("Escaped __bold__ and \\_italic\\_.", "<p>Escaped <strong>bold</strong> and _italic_.</p>\n")]
    [TestCase("Complex paragraph with __bold _italic_ and text__ inside.", "<p>Complex paragraph with <strong>bold <em>italic</em> and text</strong> inside.</p>\n")]
    public void ParagraphMarkdownElement_GetHtmlLine_ShouldReturnCorrectHtmlString(string text, string expectedHtml)
    {
        // Arrange
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
    [TestCase("#Заголовок с _курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок с __жирным__", "<h1>Заголовок с <strong>жирным</strong></h1>\n")]
    [TestCase("#Заголовок с __жирным__ и _курсивом_",
        "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em></h1>\n")]
    [TestCase("#Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>\n")]
    public void MarkdownProcessor_GetHtml_ShouldReturnCorrectHtml(string markdownText, string expectedHtml)
    {
        // Arrange
        var parser = new MarkdownParser();
        var renderer = new MarkdownRenderer();
        var markdownProcessor = new MarkdownProcessor(parser, renderer);
        // Act
        var html = markdownProcessor.GetHtml(markdownText);
        // Assert
        Assert.AreEqual(expectedHtml,html);
    }
}

using System.Security.Cryptography;

namespace MarkdownRenderText
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MarkdownRender.MDRender tag = new MarkdownRender.MDRender();
            Dictionary<string, MarkdownRender.Tag>? Tags = new Dictionary<string, MarkdownRender.Tag>{ { "#", new MarkdownRender.Tag { MarkdownTag = "#", HtmlTag = "h1", DoubleTagHtml = true } } };
            var mdText = new MarkdownRender.MDRender();
            var result = mdText.ConvertToHtml("# Заголовок с разными символами", Tags);
            var expected = "<h1> Заголовок с разными символами</h1>";
            Assert.Equal(expected, result);
        }
    }
}
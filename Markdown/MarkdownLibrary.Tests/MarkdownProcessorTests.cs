using MarkdownLibrary;
using Xunit;

namespace MarkdownLibrary.Tests
{
    public class MarkdownProcessorTests
    {
        [Fact]
        public void ConvertToHtml()
        {
            IMarkdownProcessor processor = new MarkdownProcessor();
            string markdown = "# Heading";

            string result = processor.Render(markdown);

            Assert.Equal("<h1>Heading</h1>", result);
        }
    }
}

using MarkdownRenderer.Tags;

namespace MarkdownTests
{
    public class TagTests
    {
        [Fact] 
        public void ConvertToHtmlTag_ShouldFitContentInBoldTag()
        {
            var boldTag = new BoldTag();
            var content = "Hello World!";

            var result = boldTag.ConvertToHtmlTag(content);

            Assert.Equal("<b>Hello World!</b>", result);
        }
    }
}
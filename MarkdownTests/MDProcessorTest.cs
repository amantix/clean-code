using Markdown;

namespace MarkdownTests
{
    public class Tests
    {

        public MDProcessor processor = new MDProcessor();
        public string sourceText = "";
        public string expectedOutput = "";


        [Test]
        public void Test1()
        {
            Assert.That(expectedOutput, Is.EqualTo(sourceText));
        }
    }
}
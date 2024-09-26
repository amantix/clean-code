using MarkDown.Classes;

namespace TestMD
{
    public class Tests
    {
        private MD mdProcessor;

        [SetUp]
        public void Setup()
        {
            mdProcessor = new MD();
        }

        [Test]
        public void Render_EmptyInput_ReturnsEmptyString()
        {
            string markdownText = "";
            string expectedHtml = "";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Render_Header_ReturnsCorrectHtml()
        {
            string markdownText = "# ��������� 1";
            string expectedHtml = "<h1>��������� 1</h1>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void Render_BoldText_ReturnsCorrectHtml()
        {
            string markdownText = "**������ �����**";
            string expectedHtml = "<strong>������ �����</strong>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }
    }
}
using MarkDown.Classes;

namespace TestMD
{
    public class Tests
    {
        private MdRender mdProcessor;

        [SetUp]
        public void Setup()
        {
            mdProcessor = new MdRender();
        }

        [Test]
        public void RenderEmptyInput()
        {
            string markdownText = "";
            string expectedHtml = "";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderHeader1()
        {
            string markdownText = "# Заголовок 1";
            string expectedHtml = "<h1>Заголовок 1</h1>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }
        
        [Test]
        public void RenderHeader2()
        {
            string markdownText = "## Заголовок 2";
            string expectedHtml = "<h2>Заголовок 2</h2>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }
        [Test]
        public void RenderHeader3()
        {
            string markdownText = "### Заголовок 3";
            string expectedHtml = "<h3>Заголовок 3</h3>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }
        [Test]
        public void RenderHeader4()
        {
            string markdownText = "#### Заголовок 4";
            string expectedHtml = "<h4>Заголовок 4</h4>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderHeader5()
        {
            string markdownText = "##### Заголовок 5";
            string expectedHtml = "<h5>Заголовок 5</h5>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }
        [Test]
        public void RenderHeader6()
        {
            string markdownText = "###### Заголовок 6";
            string expectedHtml = "<h6>Заголовок 6</h6>";

            string actualHtml = mdProcessor.Render(markdownText);
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderBoldText()
        {
            string markdownText = "__Жирный текст__";
            string expectedHtml = "<strong>Жирный текст</strong>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderText() 
        {
            string markdownText = "# Заголовок __с _разными_ символами__";
            string expectedHtml = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderMoreStrongText() 
        {
            string markdownText = "__Жирный текст__ и еще один __Жирный текст__";
            string expectedHtml = "<strong>Жирный текст</strong> и еще один <strong>Жирный текст</strong>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }


        [Test]
        public void RenderMoreBoldText()
        {
            string markdownText = "__Жирный _текст и еще один Жирный_ текст__";
            string expectedHtml = "<strong>Жирный <em>текст и еще один Жирный</em> текст</strong>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public void RenderStrongTextInBoldText() 
        {
            string markdownText = "_а фыв __фыв__ ф_";
            string expectedHtml = "<em>а фыв __фыв__ ф</em>";

            string actualHtml = mdProcessor.Render(markdownText);

            Assert.AreEqual(expectedHtml, actualHtml);
        }
    }
}
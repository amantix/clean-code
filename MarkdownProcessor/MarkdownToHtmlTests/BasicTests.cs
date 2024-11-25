using MDConverter;
using NUnit.Framework;

namespace MarkdownToHtmlTests;

public class Tests
{
    private MarkdownConverter _converter;
    
    [SetUp]
    public void Setup()
    {
        _converter = new MarkdownConverter();
    }

    // Тесты для обработки базовых элементов
        [TestCase("# Заголовок первого уровня", "<h1>Заголовок первого уровня</h1>")]
        [TestCase("## Заголовок второго уровня", "<h2>Заголовок второго уровня</h2>")]
        [TestCase("Текст с _курсивом_ и __жирным текстом__", "Текст с <em>курсивом</em> и <strong>жирным текстом</strong>")]
        public void ConvertMarkdownToHtml_ShouldConvertCorrectly(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        // Тесты для экранированных символов
        [TestCase("Это пример экранированного символа: \\_", "Это пример экранированного символа: _")]
        public void ConvertMarkdownToHtml_ShouldHandleEscapeSequencesCorrectly(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        // Тесты для проверки комбинированных Markdown элементов
        [TestCase("Текст с __жирным__ и _курсивом_ в одном предложении.", "Текст с <strong>жирным</strong> и <em>курсивом</em> в одном предложении.")]
        [TestCase("__Жирный и _курсив_ вместе__", "<strong>Жирный и <em>курсив</em> вместе</strong>")]
        public void ConvertMarkdownToHtml_ShouldHandleCombinedElementsCorrectly(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        // Тест для проверки корректной обработки пустых строк
        [TestCase("", "")]
        [TestCase("   ", "   ")]
        public void ConvertMarkdownToHtml_ShouldHandleEmptyOrWhitespaceStrings(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }
        
        // Тесты для сложных ситуаций с экранированными символами и Markdown-разметкой
        [TestCase("Заголовок \\#1 и нормальный заголовок", "Заголовок #1 и нормальный заголовок")]
        public void ConvertMarkdownToHtml_ShouldHandleEscapedTagsCorrectly(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        // Тесты для проверки экранирования и других символов
        [TestCase("Текст с \\*звездочками\\* и _курсивом_", "Текст с *звездочками* и <em>курсивом</em>")]
        public void ConvertMarkdownToHtml_ShouldHandleOtherEscapedSymbolsCorrectly(string markdown, string expectedHtml)
        {
            // Arrange
            var result = _converter.ConvertToHtml(markdown);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }
}
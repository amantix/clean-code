namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly InlineMarkdownParser _inlineParser;

    public MarkdownProcessor()
    {
        _inlineParser = new InlineMarkdownParser();
    }

    public string Process(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("null or empty", nameof(input));

        // обработка заголовков
        if (input.StartsWith("# "))
        {
            return $"<h1>{Process(input.Substring(2).Trim())}</h1>";
        }

        // передаем обработку элементов парсеру
        return _inlineParser.Parse(input);
    }
}

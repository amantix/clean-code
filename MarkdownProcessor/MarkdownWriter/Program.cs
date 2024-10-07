using MarkdownRenderer;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Tags;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = @"о_д_инарных__";
        string[] tokens = text.Split(' ');

        foreach (var item in tokens)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine("\n");

        var allTokens = tokensParser.ParseTokens(text);
        foreach (var token in allTokens)
        {
            Console.WriteLine($"СЛОВО - {token.Content}");
            foreach (var item in token.TagPositions)
            {
                Console.WriteLine($"Type: {item.TagType.ToString()}, " +
                                  $"Index: {item.TagIndex}, " +
                                  $"State: {item.TagState}," );
            }
        }
    }
}
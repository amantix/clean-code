using MarkdownRenderer;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Tags;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = @"__пересе_ч_ения _двойных__ и о_д_инарн__ых_";
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
using MarkdownRenderer;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Tags;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = "__bold tex__t";
        string[] tokens = text.Split(' ');

        var md = new MarkdownConverter(tokensParser);
        Console.WriteLine(md.ConvertToHtml(text)); 
    }
}
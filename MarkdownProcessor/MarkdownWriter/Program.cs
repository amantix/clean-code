using MarkdownRenderer;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Tags;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = "За __подчерками, начин_ающи_ми ____ выделение Иначе эти подчерки,__ \nНовая строка";
        string[] tokens = text.Split(' ');

        var md = new MarkdownConverter(tokensParser);
        Console.WriteLine(md.ConvertToHtml(text)); 
    }
}
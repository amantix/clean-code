using MarkdownRenderer;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = @"\_пере__сечения__ двойных_ и одинарных";
        string[] tokens = text.Split(' ');

        var md = new MarkdownConverter(tokensParser);
        Console.WriteLine(md.ConvertToHtml(text)); 
    }
}
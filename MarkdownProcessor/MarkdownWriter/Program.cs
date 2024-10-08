using MarkdownRenderer;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text = "# __двойного выделения _одинарное_ тоже__\n\n\n\nкак тебе бля";
        string[] tokens = text.Split(' ');

        var md = new MarkdownConverter(tokensParser);
        Console.WriteLine(md.ConvertToHtml(text)); 
    }
}
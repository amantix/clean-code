using MarkdownProcessorLib.Services;

namespace ProjectForTest;

internal class Program
{
    static void Main(string[] args)
    {
        var splitter = new BlockSplitter().Split("# Это заголовок");
        var str = new HtmlBuilder().Build(splitter);

        Console.WriteLine();
        foreach (var i in splitter) 
        {
            Console.Write(i.Type + " ");
        }
        Console.WriteLine("\n");
        Console.WriteLine(str.ToString()); 
        

    }
}
namespace MarkdownRealisation.Classes;

public static class Program
{
    public static void Main(string[] args)
    {
        var md = new MdProcessor(new Parser(), new MdRender(), new Resolver());
        try
        {
            Console.WriteLine( md.Process(
                "1. Первый уровень\n    1. Второй уровень\n        1. Третий уровень\n    2. Второй элемент второго уровня"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
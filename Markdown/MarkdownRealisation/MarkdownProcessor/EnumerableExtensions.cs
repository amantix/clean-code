using MarkdownRealisation.Abstractions;

namespace MarkdownRealisation;

public static class EnumerableExtensions
{
    public static void Print<T>(this IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            Console.WriteLine(item);
        }
    }

    public static void PrintTokens<T>(this IEnumerable<T> source) where T : Token
    {
        foreach (var item in source)
        {
            if (item is TagToken tagToken)
            {
                Console.WriteLine($"Type: {tagToken.Type}, Position: {tagToken.Position}, Pair: {tagToken.Pair != null}");
            }
            else
            {
                Console.WriteLine($"{item}");
            }
        }
    }

    public static void Print(this Token token)
    {
        if (token is TagToken tagToken)
        {
            Console.WriteLine($"Type: {tagToken.Type}, Position: {tagToken.Position}, Pair: {tagToken.Pair != null}");
        }
        else
        {
            Console.WriteLine($"{token}");
        }
    }
}
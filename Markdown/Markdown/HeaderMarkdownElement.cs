namespace Markdown;

// public class HeaderMarkdownElement : IMarkdownElement
// {
//     private string text;
//     private string openingTag = "<h1>";
//     private string closingTag = "</h1>";
//     public HeaderMarkdownElement(string line)
//     {
//         text = line.Substring(1);
//     }
//     public string GetHtmlLine()
//     {
//         return $"{openingTag}{text}{closingTag}";
//     }
// }

public class HeaderMarkdownElement : IMarkdownElement
{
    private IMarkdownElement nestedElement;
    private string text;
    private string openingTag = "<h1>";
    private string closingTag = "</h1>";

    public HeaderMarkdownElement(string line)
    {
        int spaceIndex = line.IndexOf("_");
        int spaceIndex2 = line.LastIndexOf("_");
        if (spaceIndex != -1)
        {
            text = line.Substring(spaceIndex,spaceIndex2-spaceIndex+1);
            nestedElement = CreateNestedMarkdownElement(text);
        }
        else
        {
            text = line.Substring(1);
        }
    }

    public string GetHtmlLine()
    {
        if (nestedElement != null)
        {
            return $"{openingTag}{nestedElement.GetHtmlLine()}{closingTag}";
        }
        else
        {
            return $"{openingTag}{text}{closingTag}";
        }
    }

    private IMarkdownElement CreateNestedMarkdownElement(string line)
    {
        if (line.StartsWith("__") && line.EndsWith("__"))
        {
            return new StrongMarkdownElement(line);
        }
        return new ItalicMarkdownElement(line);
    }
}

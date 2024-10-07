using Markdown.AbstractClasses;

namespace Markdown.BaseClasses;
public class Tokenizer
{
    public List<BaseMarkdownToken> Tokenize(string markdownText)
    {
        var tokens = new List<BaseMarkdownToken>();

        string[] markdownTextParagraphs = markdownText.Split("\n");
        foreach (var markdownTextParagraph in markdownTextParagraphs)
        {

        }

        return tokens;
    }
}

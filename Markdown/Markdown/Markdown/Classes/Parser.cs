using System.Net.Mime;
using System.Text;
using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes;

public class Parser : IParser
{
    private Dictionary<string, TagStyle> TagTypes = new Dictionary<string, TagStyle>()
    {
        { "_", TagStyle.Italic },
        { "__", TagStyle.Bold },
        { "#", TagStyle.Header },
        { @"\", TagStyle.EscapeCharacter }

    };

    public List<Token> Parse(string text)
    {
        List<Token> parsedTokens = new List<Token>();
        ExtractTags(text);
        return parsedTokens;
    }

    public List<Tag> ExtractTags(string text)
    {
        var extractedTags = new List<Tag>();
        for (int i = 0; i < text.Length; i++)
        {
            var tag = ExtractTag(text[i], i, text);
            if (tag is null)
            {
                continue;
            }

            i += tag.Length;
            extractedTags.Add(tag);
        }

        return extractedTags;
    }

    private Tag ExtractTag(char symbol, int index, string text)
    {
        if (symbol == '_')
        {
            if (index + 1 < text.Length && text[index + 1] == '_')
            {
                return new Tag(TagStyle.Bold, 2, true, index);
            }

            return new Tag(TagStyle.Italic, 1, true, index);
        }

        if (symbol == '#' && (index == 0 || text[index - 1] == '\n'))
        {
            return new Tag(TagStyle.Header, 1, false, index);
        }

        if (symbol == '\\')
        {
            return new Tag(TagStyle.EscapeCharacter, 1, false, index);
        }

        return null;

    }
    
    
    
}

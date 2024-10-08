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
        var allTagsList = ExtractTags(text);
        var tagsPairsList = ExtractTagsPairs(allTagsList);
        //проверяем парность - ExctractTagsPairs()
        //проверяем вложенность - ExctractTagsPairs()
        //проверяем корректность вложенности - ExctractTagsPairs()
        
        //проверяем пробелы
        //проверяем цифры
        
        
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

    public List<(Tag, Tag)> ExtractTagsPairs(List<Tag> tags)
    {
        var tagStack = new Stack<Tag>();
        var tagsPairsList = new List<(Tag, Tag)>();
        foreach (var tag in tags)
        {
            if (!tag.IsPaired)
            {
                continue;
            }

            if (tagStack.Count == 0 || tagStack.Peek().TagStyle != tag.TagStyle)
            {
                tagStack.Push(tag);
            }
            else
            {
                tagsPairsList.Add((tagStack.Pop(), tag));
            }
            
        }

        return tagsPairsList;
    }
    
}
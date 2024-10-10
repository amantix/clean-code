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
        var tagsWithoutEscapeCharacters = ProcessEscapeCharacters(allTagsList);
        var tagsPairsList = ExtractTagsPairs(tagsWithoutEscapeCharacters);
        //проверяем парность - ExtractTagsPairs()
        //проверяем вложенность - ExtractTagsPairs()
        //проверяем корректность вложенности - ExtractTagsPairs()
        
        //проверяем экранирование - ProcessEscapeCharacters()
        //проверить внутри слова если да, то пробелы не проверять
        //проверяем пробелы 
        //проверяем цифры - SkipTagWhenInDigitSeq()
        
        
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

    private List<Tag> ProcessEscapeCharacters(List<Tag> tags)
    {
        var result = new List<Tag>();
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i].TagStyle == TagStyle.EscapeCharacter
                && i + 1 < tags.Count
                && tags[i + 1].Index == i + 1)
            {
                i++;
                continue;
            }
            result.Add(tags[i]);
        }

        return result;
    }

    private List<(Tag, Tag)> SkipTagWhenInDigitSeq(List<(Tag, Tag)> tags, string text)
    {
        var tagsListWithValidInDigitTag = new List<(Tag, Tag)>();
        foreach (var tagPair in tags)
        {
            if (tagPair.Item1.Index != 0
                && tagPair.Item2.Index + 1 < text.Length
                && Char.IsDigit(text[tagPair.Item1.Index - 1])
                && Char.IsDigit(text[tagPair.Item2.Index + 1]))
            {
                continue;
            }
            tagsListWithValidInDigitTag.Add(tagPair);
        }

        return tagsListWithValidInDigitTag;
    }
    
    
}
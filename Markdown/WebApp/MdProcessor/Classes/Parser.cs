using MdProcessor.Abstractions;
using MdProcessor.Enums;
using MdProcessor.Interfaces;
using MdProcessor.TagsAndTokens;

namespace MdProcessor.Classes;

public class Parser : IParser
{
    private int SearchForTabulation(string line)
    {
        var result = 0;
        var tempLine = line;

        while (tempLine.StartsWith(" "))
        {
            tempLine = tempLine.Remove(0, 1);
            result++;
        }
        
        return result / 4;
    }
    private (string[] RemainingWords, TagToken OpeningTag) FindOpeningTag(string line)
    {
        (string[] RemainingWords, TagToken OpeningTag) result;
        
        var tabCount = SearchForTabulation(line);
        
        var words = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0) return ([" "], new ParagraphToken());
        
        var firstWord = words.First();
        switch (firstWord)
        {
            case "1.":
            case "2.":
            case "3.":
            case "4.":
            case "5.":
            case "6.":
            case "7.":
            case "8.":
            case "9.":
                var listTag = new ListItemToken(tabCount);
                result.OpeningTag = listTag;
                result.RemainingWords = words.Skip(1).ToArray();
                break;
            
            case "######":
            case "#####":
            case "####":
            case "###":
            case "##":
            case "#":
                var headerTag = TagLibrary.GetTagToken(firstWord);
                result.OpeningTag = headerTag;
                result.RemainingWords = words.Skip(1).ToArray();
                break;
            default:
                var paragraphTag = new ParagraphToken();
                result.OpeningTag = paragraphTag;
                result.RemainingWords = words;
                break;
        }
            
        return result;
    }

    private Token[] ParseOneWord(string word)
    {
        var result = new List<Token>();
            
        if (word.All(c => c.Equals('_')))
        {
            result.Add(new TextToken(word));
            return result.ToArray();
        }
                    
        string tempWord = word;
        foreach (var tag in TagLibrary.Tags)
        {
            if (tempWord.StartsWith(tag))
            {
                var newTag = TagLibrary.GetTagToken(tag);
                tempWord = tempWord.Substring(tag.Length);

                result.Add(newTag);
            }
        }

        var tempTokenStack = new Stack<Token>();
        foreach (var tag in TagLibrary.Tags)
        {
            if (tempWord.EndsWith(tag))
            {
                var newTag = TagLibrary.GetTagToken(tag);
                newTag.Position = TagPosition.End;
                tempWord = tempWord.Substring(0, tempWord.Length - tag.Length);
                tempTokenStack.Push(newTag);
            }
        }

        result.Add(new TextToken(tempWord));

        while (tempTokenStack.Count != 0)
        {
            result.Add(tempTokenStack.Pop());
        }

        return result.ToArray();
    }
        
    public Token[] Parse(string text)
    {
        var result = new List<Token>();

        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var lineResult = new List<Token>();
            var opening = FindOpeningTag(line);
            var words = opening.RemainingWords;
            lineResult.Add(opening.OpeningTag);
                
            foreach (var word in words)
            {
                if (word != words.First()) 
                    lineResult.Add(new TextToken(" "));

                lineResult.AddRange(ParseOneWord(word));
            }
                
            // Creating a pair for the first token
            var firstLineTagToken = (TagToken)lineResult.First();
            var lastLineTagToken = (TagToken)firstLineTagToken.Clone();
            lastLineTagToken.Position = TagPosition.End;
            lastLineTagToken.Pair = firstLineTagToken;
            firstLineTagToken.Pair = lastLineTagToken;
            lineResult.Add(lastLineTagToken);
                
            result.AddRange(lineResult.ToArray());
        }

        return result.ToArray();
    }
}
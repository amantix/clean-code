using Markdown.AbstractClasses;
using Markdown.Tags;
using System.Collections.Generic;

namespace Markdown;

public class MarkdownToHtmlBackup: IMarkdownProcessor
{
    public string Render(string markdownText)
    {
        string resultHtml = markdownText;
        resultHtml = ConvertMarkdownToHtml(markdownText);
        return resultHtml;
    }

    private string ConvertMarkdownToHtml(string markdownText)
    {
        ConvertMarkdownStringToHtml(markdownText);
        return "заглушка";
    }

    private string ConvertMarkdownStringToHtml(string input)
    {
        Dictionary<BaseMarkdownToken, int> indexesLastEncounteredTags = new Dictionary<BaseMarkdownToken, int> { { new EscapeTag(), -1 }, { new BoldTag(), -1 }, { new HeaderTag(), -1 }, { new ItalicsTag(), -1 } };
        string[] partsString = input.Split(' ');

        for (int i = 0; i < partsString.Length; i++)
        {
            string partString = partsString[i];
            for (int j = 0; j < partString.Length; j++)
            {
                if (partString.Substring(j, 1) == "_")
                {
                    var indexLastEncounteredItalicsTag = indexesLastEncounteredTags[new ItalicsTag()];
                    if (indexLastEncounteredItalicsTag != -1)
                    {
                        partString[indexLastEncounteredItalicsTag] = "_";
                    }
                    indexesLastEncounteredTags[new ItalicsTag()] = j;
                }
                else if (partString.Length - j >= 2 && partString.Substring(j, 2) == "__")
                {
                    indexesLastEncounteredTags[new BoldTag()] = j;
                }
            }
        }

        var inputString = input;
        var countOldTagInInput = GetCountSubstringInString(inputString, oldTag);
        while (countOldTagInInput > 1)
        {
            inputString = inputString.Substring(0, inputString.IndexOf(oldTag, StringComparison.Ordinal)) + newTag1 +
                          inputString.Substring(inputString.IndexOf(oldTag, StringComparison.Ordinal) + 1);
            inputString = inputString.Substring(0, inputString.IndexOf(oldTag, StringComparison.Ordinal)) + newTag2 +
                          inputString.Substring(inputString.IndexOf(oldTag, StringComparison.Ordinal) + 1);
            countOldTagInInput -= 2;
        }
        return inputString;
    }










    private string ReplaceFirstSubstringFound(string input, string oldTag, string newTag1, string newTag2)
    {
        var inputString = input;
        var countOldTagInInput = GetCountSubstringInString(inputString, oldTag);
        while (countOldTagInInput > 1)
        {
            inputString = inputString.Substring(0, inputString.IndexOf(oldTag, StringComparison.Ordinal)) + newTag1 +
                          inputString.Substring(inputString.IndexOf(oldTag, StringComparison.Ordinal) + 1);
            inputString = inputString.Substring(0, inputString.IndexOf(oldTag, StringComparison.Ordinal)) + newTag2 +
                          inputString.Substring(inputString.IndexOf(oldTag, StringComparison.Ordinal) + 1);
            countOldTagInInput -= 2;
        }
        return inputString;
    }

    private int GetCountSubstringInString(string str, string substring)
    {
        int countSubstringInString = 0;
        if (str.Contains(substring))
        {
            int substringLength = substring.Length;
            for (int i = 0; i < str.Length; i += 1)
            {
                if (str.Substring(i, substringLength) == substring)
                {
                    countSubstringInString++;
                    i += substringLength - 1;
                }
            }
        }

        return countSubstringInString;
    }
}
using System.Text;
using MarkdownRenderer.Enums;
using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer;

public class MarkdownConverter(ITokensParser parser) : IMarkdownConverter
{
    public string ConvertToHtml(string unprocessedText)
    {
        var tokens = parser.ParseTokens(unprocessedText);
        StringBuilder sb = new StringBuilder();

        foreach (var token in tokens)
        {
            var content = token.Content;
            var tagPositions = token.TagPositions;

            if (tagPositions.Count > 0)
            {
                var sortedTagPositions = tagPositions.OrderBy(tp => tp.TagIndex).ToList();
                int currentIndex = 0;

                foreach (var tagPosition in sortedTagPositions)
                {
                    sb.Append(content.Substring(currentIndex, tagPosition.TagIndex - currentIndex));

                    if (tagPosition.TagState == TagState.Open)
                    {
                        sb.Append(GetHtmlTag(tagPosition.TagType, true));
                    }
                    else if (tagPosition.TagState == TagState.Close)
                    {
                        sb.Append(GetHtmlTag(tagPosition.TagType, false));
                    }

                    currentIndex = tagPosition.TagIndex + (tagPosition.TagType == TagType.BoldTag ? 2 : 1);
                }

                if (currentIndex < content.Length)
                {
                    sb.Append(content.Substring(currentIndex));
                }
            }
            else
            {
                sb.Append(content);
            }

            sb.Append(" ");
        }

        return sb.ToString().Trim();
    }

    private string GetHtmlTag(TagType tagType, bool isOpening)
    {
        return tagType switch
        {
            TagType.ItalicTag => isOpening ? "<em>" : "</em>",
            TagType.BoldTag => isOpening ? "<strong>" : "</strong>",
            TagType.HeaderTag => isOpening ? "<h1>" : "</h1>",
            _ => ""
        };
    }
}
